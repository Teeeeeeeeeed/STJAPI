using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STJWebAppAPI.Models;
namespace STJWebAppAPI.Data
{
    public class SQLDbContext:DbContext
    {
        public SQLDbContext(DbContextOptions options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
