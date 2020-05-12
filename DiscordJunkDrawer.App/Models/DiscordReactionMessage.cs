using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DiscordJunkDrawer.App.Models
{
    class DiscordReactionMessage
    {
        public ulong Id { get; set; }
        public Discord.IUser User { get; set; }
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 30);
    }
}
