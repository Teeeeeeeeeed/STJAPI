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
        IEnumerable<Booking> GetAllBookings();
        IEnumerable<Booking> GetAllBookingsForUser(string email);
        Booking GetBookingById(int bookingId);
        Booking AddBooking(Booking booking);
        bool ValidLogin(string email, string password);
    }
}
