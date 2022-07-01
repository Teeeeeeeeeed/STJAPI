using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using STJWebAppAPI.Models;
using STJWebAppAPI.Data;
using STJWebAppAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace STJWebAppAPI.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IDbRepo _repo;
        private readonly IConfiguration _config;

        public LoginController(IDbRepo repository, IConfiguration config)
        {
            _repo = repository;
            _config = config;
        }

        //[Authorize(AuthenticationSchemes="UserHandler")]
        //[Authorize(Policy = "UserOnly")]
        [AllowAnonymous]
        [HttpPost("user")]
        public ActionResult<UserDtoOut> Login([FromBody] UserLogin user)
        {
            if (_repo.ValidLogin(user.username, user.password))
            {
                User u = _repo.GetUserByEmail(user.username);
                var token = Generate(u);
                return Ok(token);
                
            }
            return NotFound("User not found");

        }
        protected string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            if (user.IsAdmin == true)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim(ClaimTypes.Name, user.Fname),
                    new Claim(ClaimTypes.Surname, user.Lname),
                    new Claim(ClaimTypes.MobilePhone, user.Number),

                    new Claim(ClaimTypes.Role, "admin")
                };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                var claims = new[]
{
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim(ClaimTypes.Name, user.Fname),
                    new Claim(ClaimTypes.Surname, user.Lname),
                    new Claim(ClaimTypes.MobilePhone, user.Number),
                    new Claim(ClaimTypes.Role, "user")
                };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }


        [AllowAnonymous]
        [HttpPost("CreateLogin")]
        public ActionResult<UserDtoOut> CreateBooking(NewUserDtoIn newUser)
        {
            IEnumerable<User> AllUsers = _repo.GetAllUsers();
            User existingUser = AllUsers.FirstOrDefault(existing =>
            (existing.Email == newUser.Email || existing.Number == newUser.Mobile));
            if (existingUser is null)
            {
                User u = Extensions.NewUserDtoToUser(newUser);
                _repo.AddUser(u);
                var token = Generate(u);
                return Ok(token);
            }
            else
            {
                if (existingUser.Email == newUser.Email)
                {
                    return Conflict(new { 
                        message = $"Email is already in use",
                        conflictObject = "Email"
                    });
                }
                else if (existingUser.Number == newUser.Mobile)
                {
                    return Conflict(new { 
                        message = $"Mobile is already in use",
                        conflictObject = "Mobile"
                    });
                }
                else
                {
                    return Conflict(new { 
                        message = $"Unknown Error",
                        conflictObject = "Unknown"
                    });
                }
            }
        }
    }
}
