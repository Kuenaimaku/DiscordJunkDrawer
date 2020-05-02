using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DiscordJunkDrawer.Modules
{
    public class RoleModule: ModuleBase<SocketCommandContext>
    {
        List<string> blacklist = new List<string>(){"SuperUser", "Guild Master"};

        [Command("iam")]
        [Summary("Assign a role to yourself.")]
        public async Task AddRole([Remainder]string roleName)
        {
            if(blacklist.Contains(roleName)){
                await Context.Message.AddReactionAsync(new Emoji("💩"));
                return;
            }
            var _roles = Context.Guild.Roles;
            var user = Context.User;
            var requestedRole = _roles.FirstOrDefault(r => r.Name == roleName);
            var requiredRole = _roles.FirstOrDefault(r => r.Name == "SuperUser");
            
            var hasPerm = (user as IGuildUser).GuildPermissions.Has(GuildPermission.ManageRoles) || (user as SocketGuildUser).Roles.Contains(requiredRole);

            if(requestedRole == null)
            {
                if(hasPerm)
                {
                    try
                    {
                        var createdRole = await Context.Guild.CreateRoleAsync(roleName, GuildPermissions.None, null, false, null);
                        await (user as IGuildUser).AddRoleAsync(createdRole);
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
            
        [Command("inot")]
        [Summary("Remove a role from yourself")]
        public async Task RemoveRole([Remainder]string roleName)
        {
            var _roles = Context.Guild.Roles;
            var user = Context.User;
            var requestedRole = _roles.FirstOrDefault(r => r.Name == roleName);

            if(blacklist.Contains(roleName)){
                await Context.Message.AddReactionAsync(new Emoji("💩"));
                return;
            }

            if(requestedRole != null)
            {
                try
                {
                    await (user as IGuildUser).RemoveRoleAsync(requestedRole);
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

        [Command("ikill")]
        [Summary("Remove role from server")]
        public async Task DeleteRole([Remainder]string roleName)
        {
            var _roles = Context.Guild.Roles;
            var user = Context.User;
            var requestedRole = _roles.FirstOrDefault(r => r.Name == roleName);
            var requiredRole = _roles.FirstOrDefault(r => r.Name == "SuperUser");

            var hasPerm = (user as IGuildUser).GuildPermissions.Has(GuildPermission.ManageRoles) || (user as SocketGuildUser).Roles.Contains(requiredRole);

            if(blacklist.Contains(roleName))
            {
                await Context.Message.AddReactionAsync(new Emoji("💩"));
                return;
            }
            await Context.Message.AddReactionAsync(new Emoji("❓"));
            return;
        }
    }
}
