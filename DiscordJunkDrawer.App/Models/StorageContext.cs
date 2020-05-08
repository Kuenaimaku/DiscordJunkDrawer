using Microsoft.EntityFrameworkCore;

namespace DiscordJunkDrawer.App.Models
{
    public class StorageContext : DbContext
    {

        public DbSet<DiscordGuildModel> DiscordGuilds { get; set; }
        public DbSet<DiscordRoleModel> DiscordRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=data.db");
    }
}
