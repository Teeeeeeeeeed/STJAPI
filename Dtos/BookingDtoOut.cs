using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STJWebAppAPI.Dtos
{
    public class BookingDtoOut
    {
        [Required]
        public DateTimeOffset EndBookingDate { get; set; }
        [Required]
        public DateTimeOffset StartBookingDate { get; set; }

    }
}
