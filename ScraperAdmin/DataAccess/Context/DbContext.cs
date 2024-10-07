using Microsoft.EntityFrameworkCore;
using ScraperAdmin.DataAccess.Models;

namespace ScraperAdmin.DataAccess.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; } 

        public DbSet<Scraper> Scraper { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }        
    }
}
    
