using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using WebAPI.Models;

namespace WebAPI.Logic
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<GameStats> GamesStats => Set<GameStats>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<Developer> Developers => Set<Developer>();

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


            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(10, 6, 10)),
                b => b.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, entity) => $"{entity}"));
        }
    }
}
