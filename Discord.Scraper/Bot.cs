using Discord;
using Scraper_Bot.Commands.Slash;

namespace Scraper_Bot;

public class Bot : IHostedService
{
    private readonly IServiceProvider _service;
    private readonly IConfiguration _config;
    private readonly DiscordSocketClient _scraperBot;
    private readonly CommandService _commands;
    private readonly ScraperSlashCommands _slash;
    private readonly ToolsSlashCommands _toolsSlashCommands;
    private readonly SlashBuilder _slashBuilder;

    private readonly Browser _browser;


    private readonly string PREFIX;
    private readonly string KEY;

    public Bot(IServiceProvider service, IConfiguration config)
    {
        _service = service;
        _config = config;
        _scraperBot = service.GetRequiredService<DiscordSocketClient>();
        _scraperBot.PurgeUserCache();
        _commands = service.GetRequiredService<CommandService>();
        _slash = service.GetRequiredService<ScraperSlashCommands>();
        _toolsSlashCommands = service.GetRequiredService<ToolsSlashCommands>();
        _slashBuilder = service.GetRequiredService<SlashBuilder>();

        _browser = service.GetRequiredService<Browser>();

        PREFIX = config["Prefix"];
        KEY = config["DiscordKeys:Token"];
    }

    private void BotEvents()
    {
        _scraperBot.Log += ScraperClient_Log;
        _scraperBot.Ready += ScraperClient_Ready;
        _scraperBot.SlashCommandExecuted += ScraperClient_SlashCommandExecuted;
    }



    private async Task ScraperClient_SlashCommandExecuted(SocketSlashCommand arg)
    {
        if (arg.Data.Name.Equals("scraper"))
        {
            var option = arg.Data.Options.FirstOrDefault();
            switch (option.Name)
            {
                case "crunchyroll":
                    await _slash.Crunchyroll(arg);
                    break;
                case "imdb":

                    break;
                case "steam":
                    await _slash.Steam(arg);
                    break;
            }
        }
        else if (arg.Data.Name.Equals("tools"))
        {
            ulong id = arg.ChannelId.Value;
            var guildId = ulong.Parse(_config["GuildID"]);
            var guild = _scraperBot.GetGuild(guildId);
            var channel = guild.GetTextChannel(id);
            await _toolsSlashCommands.Purge(arg, channel);
        }
        else if(arg.Data.Name.Equals("lfg"))
        {
            var guildId = ulong.Parse(_config["GuildID"]);
            var guild = _scraperBot.GetGuild(guildId);
            //var channel = guild.GetThreadChannel(1081483395701411900);
            var channel = guild.GetForumChannel(1081483395701411900);

            await _toolsSlashCommands.LFG(arg, channel);
        }
    }

    private async Task ScraperClient_Ready()
    {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
        await _scraperBot.SetActivityAsync(new Discord.Game("der Scraper Rolle ", ActivityType.Listening));

        try
        {
            await _slashBuilder
                   .Start().Result
                   .Tools().Result
                   .LFG().Result
                   .Scraper();
        }
        catch (Exception err)
        {

            Console.WriteLine(err.Message);
        }
    }

    private async Task ScraperClient_Log(LogMessage arg)
    {
        var channel = _scraperBot.GetChannelAsync(1078987423398236221).Result as IMessageChannel;

        await channel.SendMessageAsync(arg.Message);
        Log.Logger.Information(arg.Message);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {

        BotEvents();

        await _scraperBot.LoginAsync(TokenType.Bot, KEY);

        await _scraperBot.StartAsync();

        Console.ReadKey();
        await _scraperBot.LogoutAsync();
        await _scraperBot.StopAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _scraperBot.StopAsync();
    }
}
