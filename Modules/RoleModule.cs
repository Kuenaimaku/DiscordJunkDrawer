using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordJunkDrawer.Models;
using DiscordJunkDrawer.Repositories;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordJunkDrawer.Modules
{
    public class RoleModule: ModuleBase<SocketCommandContext>
    {
        private readonly IConfigurationRoot _config;

        public RoleModule(IConfigurationRoot config)
        {
            _config = config;
        }

        [Command("iam")]
        [Summary("Assign a role to yourself.")]
        public async Task AddRole([Remainder]string text)
        {
            var _roles = Context.Guild.Roles;
            var _allowedRoles = await GetRolesByGuildAsync(Context.Guild.Id.ToString());
            var user = Context.User;
            var requestedRole = _roles.FirstOrDefault(r => r.Name == text);
            if (requestedRole != null && _allowedRoles.Any(r => r.Id == requestedRole.Id.ToString()))
            {
                try
                {
                    await (user as IGuildUser).AddRoleAsync(requestedRole);
                    await Context.Message.AddReactionAsync(new Emoji("✅"));
                    return;
                }
                catch
                {
                    await Context.Message.AddReactionAsync(new Emoji("❌"));
                    return;
                }
            }

            await Context.Message.AddReactionAsync(new Emoji("❌"));
            return;
        }

        [Command("addrole")]
        public async void CreateRole([Remainder]string text)
        {
            var guildRoles = Context.Guild.Roles;

            List<string> r = new List<string> { "a", "test" };
            var test = r.Contains("a");


            var guild = Context.Guild;
            var requestedRole = guildRoles.FirstOrDefault(x => x.Name == text);
            
            if (requestedRole == null)
            {
                await guild.CreateRoleAsync(text, null, null, false, null);
            }
            
            requestedRole = guildRoles.FirstOrDefault(x => x.Name == text);
            var allowedRoles = await GetRolesByGuildAsync(guild.Id.ToString());
            if (requestedRole != null && !allowedRoles.Any(x => x.Id == requestedRole.Id.ToString()))
            {
                var role = new Role
                {
                    Id = requestedRole.Id.ToString(),
                    GuildId = guild.Id.ToString(),
                    Name = requestedRole.Name
                };

                var resp = await AddRoleToGuildAsync(role);

                if (resp)
                {
                    await Context.Message.AddReactionAsync(new Emoji("✅"));
                    return;
                }
                else
                {
                    await Context.Message.AddReactionAsync(new Emoji("❌"));
                    return;
                }
            }

        }

        private async Task<List<Role>> GetRolesByGuildAsync(string guildId)
        {
            using(var db = new DataContext(_config))
            {
                db.Roles.Add(new Role { Id="1", GuildId = "1", Name ="birb" });
                db.SaveChanges();
                var data = db.Roles.AsQueryable().Where(r => r.GuildId == guildId).ToList();
                return await Task.FromResult(data);
            }
        }

        private async Task<bool> AddRoleToGuildAsync(Role role)
        {
            using (var db = new DataContext(_config))
            {
                if (db.Roles.FirstOrDefaultAsync(x => x.Id == role.Id && x.GuildId == role.GuildId) == null)
                {
                    await db.Roles.AddAsync(role);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
            
    }
}
