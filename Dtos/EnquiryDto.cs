using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STJWebAppAPI.Dtos
{
    public class EnquiryDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Department { get; set; }
    }
}
