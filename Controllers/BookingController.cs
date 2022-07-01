using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using STJWebAppAPI.Models;
using STJWebAppAPI.Data;
using STJWebAppAPI.Dtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace STJWebAppAPI.Controllers
{
    [Route("bookings")]
    [ApiController]
    public class BookingController:Controller
    {
        private readonly IDbRepo _repo;

        public BookingController(IDbRepo repository)
        {
            _repo = repository;
        }

        [HttpGet("GetBookings")]
        public ActionResult<IEnumerable<BookingDtoOut>> GetBookings()
        {
            IEnumerable<BookingDtoOut> bookings = _repo.GetAllBookings().Select(booking =>Extensions.BookingToDtoOut(booking));
            return Ok(bookings);
        }
        [HttpPost("CreateBooking")]
        public ActionResult<BookingDtoOut> CreateBooking(BookingDtoIn booking)
        {
            IEnumerable<Booking> AllBookings = _repo.GetAllBookings();
            Booking overlappedBooking = AllBookings.FirstOrDefault(existing =>
            (existing.StartBookingDate <= booking.StartBookingDate && existing.EndBookingDate >= booking.StartBookingDate) ||
            (existing.StartBookingDate <= booking.EndBookingDate && existing.EndBookingDate >= booking.EndBookingDate)
            );
            if (overlappedBooking is null)
            {
                Booking newBooking = Extensions.BookingDtoInToBooking(booking);
                _repo.AddBooking(newBooking);
                return Ok(Extensions.BookingToDtoOut(newBooking));
                
            }
            else
            {
                return Conflict(new { message = $"There is an existing booking in the time range" });
            }
        }

        [HttpGet("userInfo")]
        [Authorize]
        public ActionResult<UserDtoOut> GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                var isAdmin = false;
                if (userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value == "admin")
                {
                    isAdmin = true;
                }
                return Ok(new UserDtoOut
                {
                    FirstName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    LastName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Mobile = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.MobilePhone)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    isAdmin = isAdmin
                });
            }
            return NotFound("User not found");
        }


        [HttpGet("userBookings")]
        [Authorize(Roles = "user")]
        public ActionResult<UserBookings> GetUserBookings()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null )
            {
                var email = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                IEnumerable<Booking> userBookings = _repo.GetAllBookingsForUser(email);
                IEnumerable<BookingDtoOut> otherBookings = _repo.GetOtherBookingsForUser(email).Select(val => {
                    return Extensions.BookingToDtoOut(val);
                });
                return Ok(new UserBookings { 
                    userBookings=userBookings,
                    otherBookings=otherBookings
                });
            }
            return NotFound("No Permissions");
        }

        [HttpGet("adminBookings")]
        [Authorize(Roles = "admin")]
        public ActionResult<List<Booking>> GetAdminBookings()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                List<Booking> bookings = _repo.GetAllBookings().ToList();
                return Ok(bookings);
            }
            return NotFound("No Permissions");
        }

        [HttpPost("delete")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteBooking(Booking booking)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                if (_repo.DeleteBooking(booking)==true)
                {
                    return Ok();
                }
                return NotFound(new { message = $"Booking does not exist" });
            }
            return NotFound("No Permissions");
        }

        [HttpPost("updateUserBookings")]
        [Authorize(Roles = "user,admin")]
        public ActionResult<BookingDtoOut> UpdateBooking(Booking booking)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var email = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                Booking b = _repo.GetBookingById(booking.BookingId);
                if (email == b.Email)
                {
                    //No change in date and just change in comments etc.
                    if(booking.StartBookingDate == b.StartBookingDate)
                    {
                        Booking newBooking = _repo.UpdateBooking(booking);
                        return Ok(newBooking);
                    }
                    //Change in date, check for clashes
                    IEnumerable<Booking> all = _repo.GetAllBookings();
                    if (all.FirstOrDefault(existing => existing.StartBookingDate == booking.StartBookingDate) == default)
                    {
                        Booking newBooking = _repo.UpdateBooking(booking);
                        return Ok(newBooking);
                    }
                    else
                    {
                        return Conflict(new { message = $"Clash in booking times" });
                    }
                }
                else
                {
                    return Conflict(new { message = $"Cannot edit different users booking" });
                }
            }
            else
            {
                return Conflict(new { message = $"No Identity Claim" });
            }
        }
    }
}
