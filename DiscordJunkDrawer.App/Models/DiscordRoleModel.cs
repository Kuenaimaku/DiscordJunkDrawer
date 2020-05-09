using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DiscordJunkDrawer.App.Models
{
    public class DiscordRoleModel
    {
        public ulong Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Guild")]
        public ulong GuildId { get; set; }
        public virtual DiscordGuildModel Guild { get; set; }
    }
}
