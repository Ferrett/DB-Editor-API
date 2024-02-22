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
        public DbSet<UserGame> UserGame => Set<UserGame>();

        public ApplicationDbContext()
        {
            Database.EnsureCreated();
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGame>()
                .HasKey(sc => new { sc.UserID, sc.GameID});

            modelBuilder.Entity<UserGame>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserGames)
                .HasForeignKey(sc => sc.UserID);

            modelBuilder.Entity<UserGame>()
                .HasOne(sc => sc.Game)
                .WithMany(c => c.UserGames)
                .HasForeignKey(sc => sc.GameID);
        }
    }
}
