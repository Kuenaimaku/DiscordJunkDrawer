using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordJunkDrawer.App.Interfaces;
using DiscordJunkDrawer.App.Models;
using DiscordJunkDrawer.App.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordJunkDrawer.App.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        private readonly IRepository<DiscordGuildModel> _guildRepository;
        private readonly IRepository<DiscordRoleModel> _roleRepository;

        // DiscordSocketClient, CommandService, and IConfigurationRoot are injected automatically from the IServiceProvider
        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commands,
            IConfigurationRoot config,
            IRepository<DiscordGuildModel> guildRepository,
            IRepository<DiscordRoleModel> roleRepository
            )
        {
            _provider = provider;
            _config = config;
            _discord = discord;
            _commands = commands;

            _guildRepository = guildRepository;
            _roleRepository = roleRepository;
        }

        public async Task StartAsync()
        {
            string discordToken = _config["tokens:discord"];     // Get the discord token from the config file
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Please enter your bot's token into the `_config.yml` file found in the applications root directory.");

            await _discord.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
            await _discord.StartAsync().ConfigureAwait(false);                                // Connect to the websocket

            _discord.Ready += OnClientReady;
            _discord.JoinedGuild += OnClientJoinedGuild;
            _discord.LeftGuild += OnClientLeftGuild;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);     // Load commands and modules into the command service
        }

        public async Task OnClientReady()
        {
            var connectedGuilds = _discord.Guilds;


                var storedGuilds = await _guildRepository.GetAllAsync();
                foreach (var connectedGuild in connectedGuilds)
                {
                    if (!storedGuilds.Any(x => x.Id == connectedGuild.Id))
                    {
                        var guildToAdd = new DiscordGuildModel
                        {
                            Id = connectedGuild.Id,
                            Name = connectedGuild.Name,
                        };

                        Console.WriteLine($"DEBUG: found connected DiscordGuild that needs to be added to local db: {JsonConvert.SerializeObject(guildToAdd)}");
                    }
                }

        }

        public async Task OnClientJoinedGuild(SocketGuild joinedGuild)
        {
            var storedGuilds = await _guildRepository.GetAllAsync();
            if (!storedGuilds.Any(x => x.Id == joinedGuild.Id))
            {
                var guildToAdd = new DiscordGuildModel
                {
                    Id = joinedGuild.Id,
                    Name = joinedGuild.Name
                };
                await _guildRepository.AddAsync(guildToAdd);
            }
        }

        public async Task OnClientLeftGuild(SocketGuild leftGuild)
        {
            var storedGuild = await _guildRepository.GetAsync(x => x.Id == leftGuild.Id);
            if (storedGuild != null)
            {
                var allRoles = await _roleRepository.GetAllAsync();
                var guildRoles = allRoles.Where(x => x.ServerId == storedGuild.Id);
                await _roleRepository.RemoveAsync(guildRoles);
                await _guildRepository.RemoveAsync(storedGuild);
            }
        }
    }
}