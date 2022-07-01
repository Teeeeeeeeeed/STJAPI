using STJWebAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STJWebAppAPI.Dtos
{
    public class AdminBookings
    {
        public IEnumerable<IEnumerable<Booking>> Bookings { get; set; }
    }
}
