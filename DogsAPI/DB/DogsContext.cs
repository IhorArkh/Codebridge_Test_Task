using DogsAPI.DB.Configuration;
using DogsAPI.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DogsAPI.DB
{
    public class DogsContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }

        public DogsContext(DbContextOptions<DogsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DogConfiguration());
        }
    }
}
