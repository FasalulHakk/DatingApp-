using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataConetxt : DbContext
    {
        public DataConetxt(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}