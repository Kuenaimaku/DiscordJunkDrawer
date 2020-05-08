using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using DiscordJunkDrawer.Models;

namespace DiscordJunkDrawer.Modules
{
    public class RoleModule: ModuleBase<SocketCommandContext>
    {
        List<string> blacklist = new List<string>(){"SuperUser"};

        [Command("iam")]
        [Summary("Assign a role to yourself.")]
        public async Task AddRole([Remainder]string roleName)
        {
            var _roles = Context.Guild.Roles;
            var user = Context.User;
            var blankRole = new GuildPermissions();
            var requestedRole = _roles.FirstOrDefault(r => r.Name == roleName);

            if(requestedRole != null)
            {
                if(!requestedRole.Permissions.Equals(blankRole) || blacklist.Contains(roleName))
                {
                    await Context.Message.AddReactionAsync(new Emoji("💩"));
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
            await Context.Message.AddReactionAsync(new Emoji("❓"));
            return;
        }
            
        [Command("Create")]
        [Summary("Create a role for the server")]
        public async Task CreateRole([Remainder]string roleName)
        {

            var _roles = Context.Guild.Roles;
            var user = Context.User;
            var requestedRole = _roles.FirstOrDefault(r => r.Name == roleName);
            var requiredRole = _roles.FirstOrDefault(r => r.Name == "SuperUser");
            
            var hasPerm = (user as IGuildUser).GuildPermissions.Has(GuildPermission.ManageRoles) || (user as SocketGuildUser).Roles.Contains(requiredRole);

            if(hasPerm)
            {
                try
                {
                    using (var db = new StorageContext())
                    {
                        var storedGuilds = await db.DiscordGuilds.ToListAsync();
                        var curGuild = storedGuilds.Find(guild => guild.Id == Context.Guild.Id);
                        var createdRole = await Context.Guild.CreateRoleAsync(roleName, GuildPermissions.None, null, false, null);

                        DiscordRoleModel roleToAdd = new DiscordRoleModel()
                        {
                            Id = createdRole.Id,
                            Name = roleName,
                            ServerId = Context.Guild.Id
                        };
                        curGuild.Roles.Add(roleToAdd);    
                        
                        await db.DiscordRoles.AddAsync(roleToAdd);     
                        await db.SaveChangesAsync();
                    }

                    await Context.Message.AddReactionAsync(new Emoji("✅"));
                    return;
                }
                catch
                {
                    await Context.Message.AddReactionAsync(new Emoji("❌"));
                    return;
                }
            }
            await Context.Message.AddReactionAsync(new Emoji("💩"));
            return;
        }

        [Command("iamnot")]
        [Alias("iaint")]
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

        [Command("Remove")]
        [Summary("Remove a role from the server")]
        public async Task DeleteRole([Remainder]string roleName)
        {
            var _roles = Context.Guild.Roles;
            var user = Context.User;
            var blankRole = new GuildPermissions();
            var requestedRole = _roles.FirstOrDefault(r => r.Name == roleName);
            var requiredRole = _roles.FirstOrDefault(r => r.Name == "SuperUser");

            var hasPerm = (user as IGuildUser).GuildPermissions.Has(GuildPermission.ManageRoles) || (user as SocketGuildUser).Roles.Contains(requiredRole);
            if(blacklist.Contains(roleName)|| !requestedRole.Permissions.Equals(blankRole) )
            {
                await Context.Message.AddReactionAsync(new Emoji("💩"));
                return;
            }
            if(hasPerm)
            {
                if(_roles.Any(x => x.Name == roleName))
                {
                    try
                    {
                        using (var db = new StorageContext())
                        {
                            var roleToDelete = await db.DiscordRoles.FirstOrDefaultAsync(x => x.Name == roleName);
                            db.DiscordRoles.Remove(roleToDelete);
                            await db.SaveChangesAsync();
                            await requestedRole.DeleteAsync();
                            await Context.Message.AddReactionAsync(new Emoji("✅"));
                            return;
                        }
                    }
                    catch
                    {
                        await Context.Message.AddReactionAsync(new Emoji("❌"));
                    }
                }
            }
            await Context.Message.AddReactionAsync(new Emoji("❓"));
            return;
        }
    }
}
