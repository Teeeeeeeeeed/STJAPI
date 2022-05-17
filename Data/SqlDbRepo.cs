using STJWebAppAPI.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading.Tasks;

namespace STJWebAppAPI.Data
{
    public class SqlDbRepo : IDbRepo
    {
        private readonly SQLDbContext _dbContext;
        public SqlDbRepo(SQLDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Booking AddBooking(Booking booking)
        {
            EntityEntry<Booking> e = _dbContext.Bookings.Add(booking);
            Booking b = e.Entity;
            _dbContext.SaveChanges();
            return b;
        }

        public User AddUser(User user)
        {
            EntityEntry<User> e = _dbContext.Users.Add(user);
            User u = e.Entity;
            _dbContext.SaveChanges();
            return u;
        }
        public Booking RemoveBooking(Booking booking)
        {
            EntityEntry<Booking> e = _dbContext.Bookings.Remove(booking);
            Booking b = e.Entity;
            _dbContext.SaveChanges();
            return b;
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            IEnumerable<Booking> bookings = _dbContext.Bookings.ToList<Booking>();
            return bookings;
        }

        public IEnumerable<Booking> GetAllBookingsForUser(string email)
        {
            IEnumerable<Booking> bookings = _dbContext.Bookings.Where(book => book.Email==email).ToList();
            return bookings;
        }

        public IEnumerable<User> GetAllUsers()
        {
            IEnumerable<User> users = _dbContext.Users.ToList();
            return users;
        }

        public Booking GetBookingById(int bookingId)
        {
            Booking b = _dbContext.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            return b;
        }

        public User GetUserByEmail(string email)
        {
            User user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public bool ValidLogin(string email, string password)
        {
            User u = _dbContext.Users.FirstOrDefault(e => String.Equals(e.Email, email) && String.Equals(e.Password, password));
            if (u == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
