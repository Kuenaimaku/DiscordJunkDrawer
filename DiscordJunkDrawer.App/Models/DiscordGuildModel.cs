using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordJunkDrawer.App.Models
{
    public class DiscordGuildModel
    {
        public ulong Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("GuildId")]
        public virtual List<DiscordRoleModel> Roles { get; set; } = new List<DiscordRoleModel>();
    }
}
