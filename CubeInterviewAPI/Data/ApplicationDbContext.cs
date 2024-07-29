using Microsoft.EntityFrameworkCore;
using CubeInterviewAPI.Models;

namespace CubeInterviewAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Currency> Currencies { get; set; }
    }
}
