using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordJunkDrawer.Modules
{
    public class RoleModule: ModuleBase<SocketCommandContext>
    {
        [Command("iam")]
        [Summary("Assign a role to yourself.")]
        public Task AddRole([Remainder]string text)
        {
            var _roles = Context.Guild.Roles;
            var user = Context.User;
            var requestedRole = _roles.FirstOrDefault(r => r.Name == text);
            if (requestedRole != null)
            {
                try
                {
                    (user as IGuildUser).AddRoleAsync(requestedRole);
                    return Context.Message.AddReactionAsync(new Emoji("✅"));
                }
                catch
                {
                    return Context.Message.AddReactionAsync(new Emoji("❌"));
                }
            }

            return Context.Message.AddReactionAsync(new Emoji("❌"));

        }
            
    }
}
