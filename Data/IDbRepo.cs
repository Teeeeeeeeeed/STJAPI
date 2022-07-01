using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STJWebAppAPI.Models;
namespace STJWebAppAPI.Data
{
    public interface IDbRepo
    {
        IEnumerable<User> GetAllUsers();
        User GetUserByEmail(string email);
        User AddUser(User user);
        Booking RemoveBooking(Booking booking);
        Booking UpdateBooking(Booking booking);
        IEnumerable<Booking> GetAllBookings();
        IEnumerable<Booking> GetAllBookingsForUser(string email);
        IEnumerable<Booking> GetOtherBookingsForUser(string email);
        Booking GetBookingById(int bookingId);
        Booking AddBooking(Booking booking);
        Boolean DeleteBooking(Booking booking);
        bool ValidLogin(string email, string password);
    }
}
