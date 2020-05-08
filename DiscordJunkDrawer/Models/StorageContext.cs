using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordJunkDrawer.Models
{
    public class StorageContext : DbContext
    {

        public DbSet<DiscordGuildModel> DiscordGuilds { get; set; }
        public DbSet<DiscordRoleModel> DiscordRoles { get; set; }
    }
}
