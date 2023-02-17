using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Logic
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> User => Set<User>();
        public DbSet<Review> Review => Set<Review>();
        public DbSet<GameStats> GameStats => Set<GameStats>();
        public DbSet<Game> Game => Set<Game>();
        public DbSet<Developer> Developer => Set<Developer>();

        public ApplicationContext()
        {
            Database.EnsureCreated();
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
