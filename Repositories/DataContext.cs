using Microsoft.EntityFrameworkCore;

using DiscordJunkDrawer.Models;
using Microsoft.Extensions.Configuration;

namespace DiscordJunkDrawer.Repositories
{
    public class DataContext : DbContext
    {
        private string _connectionString;

        public DataContext() : base() { }

        public DataContext(IConfigurationRoot config)
        {
            _connectionString = config["connections:sqlite"];
        }

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Role> Roles { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./data.db");
        }
    }
}
