using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordJunkDrawer.Models
{
    public class storageContext : DbContext
    {

        public DbSet<DiscordGuildModel> DiscordGuilds { get; set; }
        public DbSet<DiscordRoleModel> DiscordRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=data.db");
    }
}
