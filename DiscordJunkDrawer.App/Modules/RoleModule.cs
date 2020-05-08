using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using DiscordJunkDrawer.App.Models;
using DiscordJunkDrawer.App.Interfaces;

namespace DiscordJunkDrawer.App.Modules
{
    public class RoleModule: ModuleBase<SocketCommandContext>
    {
        private readonly IRepository<DiscordRoleModel> _roleRepository;
        private readonly IRepository<DiscordGuildModel> _guildRepository;
        public RoleModule(IRepository<DiscordRoleModel> roleRepository, IRepository<DiscordGuildModel> guildRepository) : base()
        {
            _roleRepository = roleRepository;
            _guildRepository = guildRepository;
        }

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
                    var curGuild = await _guildRepository.GetAsync(guild => guild.Id == Context.Guild.Id);
                    var createdRole = await Context.Guild.CreateRoleAsync(roleName, GuildPermissions.None, null, false, null);

                    DiscordRoleModel roleToAdd = new DiscordRoleModel()
                    {
                        Id = createdRole.Id,
                        Name = roleName,
                        ServerId = Context.Guild.Id
                    };
                    curGuild.Roles.Add(roleToAdd);
                    await _guildRepository.UpdateAsync(curGuild);
                    await _roleRepository.AddAsync(roleToAdd);
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

                        var roleToDelete = await _roleRepository.GetAsync(x => x.Name == roleName);
                        await _roleRepository.RemoveAsync(roleToDelete);
                        await requestedRole.DeleteAsync();
                        await Context.Message.AddReactionAsync(new Emoji("✅"));
                        return;

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
