using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebAPI.Models;

namespace WebAPI.Logic
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> User => Set<User>();
        public DbSet<Review> Review => Set<Review>();
        public DbSet<GameStats> GameStats => Set<GameStats>();
        public DbSet<Game> Game => Set<Game>();
        public DbSet<Developer> Developer => Set<Developer>();

        public ApplicationDbContext()
        {
            //Database.EnsureCreated();
            //Database.EnsureDeleted();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
