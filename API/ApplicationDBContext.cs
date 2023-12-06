using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { 
        }

        public DbSet<Address> Addresses { get; set; }
    }
}
