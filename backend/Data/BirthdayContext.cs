using Microsoft.EntityFrameworkCore;
using Congratz.backend.Models;

namespace Congratz.backend.Context
{
    public class BirthdayContext : DbContext
    {
        public BirthdayContext(DbContextOptions<BirthdayContext> options) : base(options) { }

        public DbSet<BirthdayPerson> Persons { get; set; }
    }
}