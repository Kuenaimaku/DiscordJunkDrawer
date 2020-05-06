using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordJunkDrawer.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordJunkDrawer.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        // DiscordSocketClient, CommandService, and IConfigurationRoot are injected automatically from the IServiceProvider
        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commands,
            IConfigurationRoot config)
        {
            _provider = provider;
            _config = config;
            _discord = discord;
            _commands = commands;
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

            using (var db = new storageContext())
            {
                var storedGuilds = await db.DiscordGuilds.ToListAsync();
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

        }

        public async Task OnClientJoinedGuild(SocketGuild joinedGuild)
        {

            using (var db = new storageContext())
            {
                var storedGuilds = await db.DiscordGuilds.ToListAsync();
                if (!storedGuilds.Any(x => x.Id == joinedGuild.Id))
                {
                    var guildToAdd = new DiscordGuildModel
                    {
                        Id = joinedGuild.Id,
                        Name = joinedGuild.Name
                    };
                    await db.DiscordGuilds.AddAsync(guildToAdd);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task OnClientLeftGuild(SocketGuild leftGuild)
        {

            using (var db = new storageContext())
            {
                var storedGuild = await db.DiscordGuilds.FirstOrDefaultAsync(x => x.Id == leftGuild.Id);
                if (storedGuild != null)
                {
                    db.DiscordGuilds.Remove(storedGuild);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}