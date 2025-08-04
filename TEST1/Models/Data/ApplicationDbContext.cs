using Microsoft.EntityFrameworkCore;
using TEST1.Models;

namespace TEST1.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-98HM803\\SQLEXPRESS01; database=intern; Integrated Security=True; TrustServerCertificate=True;"); }

        public DbSet<Product> Products { get; set; } 
        public DbSet<Category> Categories { get; set; }
    }
}
