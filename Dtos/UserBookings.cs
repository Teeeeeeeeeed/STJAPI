using STJWebAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STJWebAppAPI.Dtos
{
    public class UserBookings
    {
        public IEnumerable<Booking> userBookings { get; set; }
        public IEnumerable<BookingDtoOut> otherBookings { get; set; }
    }
}
