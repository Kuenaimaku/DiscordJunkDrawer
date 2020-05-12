using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using DiscordJunkDrawer.App.Models;
using DiscordJunkDrawer.App.Interfaces;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using System.Linq;
using System;

namespace DiscordJunkDrawer.App.Modules
{
    [Group("settings")]
    public class SettingsModule : InteractiveBase<SocketCommandContext>
    {
        private readonly IRepository<DiscordRoleModel> _roleRepository;
        private readonly IRepository<DiscordGuildModel> _guildRepository;

        private readonly Emoji _confirmEmoji = new Emoji("✅");
        private readonly Emoji _denyEmoji = new Emoji("❌");


        public SettingsModule(IRepository<DiscordRoleModel> roleRepository, IRepository<DiscordGuildModel> guildRepository) : base()
        {
            _roleRepository = roleRepository;
            _guildRepository = guildRepository;
        }

        [Command("cleanup")]
        [Summary("Remove all roles controlled by the bot, and leave the server.")]
        public async Task CleanupBotAsync()
        {
            await InlineReactionReplyAsync(new ReactionCallbackData($"This will remove all roles handled by {Context.User.Username}. Please press the Checkbox to confirm, or the Cross mark to cancel.",
                null, true, true, TimeSpan.FromSeconds(20), (c) => c.Channel.SendMessageAsync("Timed Out!"))
                .WithCallback(new Emoji("✔️"), async (c, r) => await Leave(c,r))
                .WithCallback(new Emoji("❌"), (c, r) => c.Channel.SendMessageAsync($"{r.User.Value.Mention} replied with 👎")),
                true
            );
        }

        private async Task Leave(SocketCommandContext context, SocketReaction reaction)
        {
            await context.Message.AddReactionAsync(new Emoji("👋"));

            var guild = context.Guild;
            var rolesInGuild = context.Guild.Roles;

            var guildToDelete = await _guildRepository.GetAsync(x => x.Id == guild.Id);
            var rolesToDelete = guildToDelete.Roles;
            foreach (var role in rolesToDelete)
            {
                var roleToDelete = rolesInGuild.FirstOrDefault(x => x.Id == role.Id);
                if (roleToDelete != null)
                {
                    await roleToDelete.DeleteAsync();
                }
            }

            await _roleRepository.RemoveAsync(rolesToDelete);
            await _guildRepository.RemoveAsync(guildToDelete);

            await(Context.Guild as IGuild).LeaveAsync();

        }
    }
}
