using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STJWebAppAPI.Dtos
{
    public class BookingDtoIn
    {
        [Required]
        public string FName { get; set; }
        [Required]
        public string Lname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public DateTimeOffset EndBookingDate { get; set; }
        [Required]
        public DateTimeOffset StartBookingDate { get; set; }
        [Required]
        public string Service { get; set; }
        public string comments { get; set; }
    }
}
