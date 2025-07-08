using Microsoft.EntityFrameworkCore;
using BirthdayApp.Models;

namespace BirthdayApp.Data
{
    public class BirthdayContext : DbContext
    {
        public BirthdayContext(DbContextOptions<BirthdayContext> options) : base(options) { }

        public DbSet<BirthdayPerson> Persons { get; set; }
    }
}