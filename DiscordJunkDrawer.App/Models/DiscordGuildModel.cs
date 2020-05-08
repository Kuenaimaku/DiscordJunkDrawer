using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordJunkDrawer.App.Models
{
    public class DiscordGuildModel
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public List<DiscordRoleModel> Roles { get; } = new List<DiscordRoleModel>();
    }
}
