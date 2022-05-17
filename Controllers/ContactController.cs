using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using STJWebAppAPI.Dtos;
using Microsoft.AspNetCore.Html;

namespace STJWebAppAPI.Controllers
{
    
    [ApiController]
    public class ContactController:Controller
    {
        [HttpPost("contact")]
        public async Task<IActionResult> EnquiryEmailAsync(EnquiryDto enquiry)
        {
            SmtpClient client = new SmtpClient("in-v3.mailjet.com", 587);
            client.Credentials = new NetworkCredential("db9be3189e77b3dd0c245fcaebec0863", "ba9f00f283d97f7095031e31def024d8");
            string builder = string.Format("<html><h3>{0} - {1}</h3><p>{2}</p>From - <a>Email - {3}</a><p>Contact Number - {4}</p><p>Name - {5}</p></html>",enquiry.Title,enquiry.Department,enquiry.Message,enquiry.Email,enquiry.Number,enquiry.Name);
            MailAddress FromEmail = new MailAddress("ted.lo.stj@gmail.com", "STJ Admin");
            MailAddress ToEmail = new MailAddress("ted.te.lo@hotmail.com", "Ted Lo");
            MailMessage Message = new MailMessage()
            {
                From = FromEmail,
                Subject = string.Format("{0} - {1}",enquiry.Department,enquiry.Title),
                Body = builder,
                IsBodyHtml = true
            };
            Message.To.Add(ToEmail);

            try
            {
                client.Send(Message);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }
            
        }
    }
}
