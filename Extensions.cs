using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STJWebAppAPI.Models;
using STJWebAppAPI.Dtos;
namespace STJWebAppAPI
{
    public static class Extensions
    {
        public static BookingDtoOut BookingToDtoOut(Booking booking)
        {
            BookingDtoOut b = new()
            {
                EndBookingDate = booking.EndBookingDate,
                StartBookingDate = booking.StartBookingDate
            };
            return b;
        }

        public static Booking BookingDtoInToBooking(BookingDtoIn booking)
        {
            Booking b = new()
            {
                FName = booking.FName,
                Lname = booking.Lname,
                Email = booking.Email,
                Number = booking.Number,
                StartBookingDate = booking.StartBookingDate,
                EndBookingDate = booking.EndBookingDate,
                Service = booking.Service,
                comments = booking.comments,
                CreatedDate = DateTimeOffset.UtcNow
            };
            return b;
        }
    }
}
