using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using STJWebAppAPI.Models;
using STJWebAppAPI.Data;
using STJWebAppAPI.Dtos;

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
    }
}
