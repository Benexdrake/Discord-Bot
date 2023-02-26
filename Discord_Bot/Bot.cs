using Discord;
using Discord_Bot.Commands.Slash;
using Webscraper_API.Scraper.Dota2.Models;

namespace Discord_Bot;

public class Bot : IHostedService
{
    private readonly IServiceProvider _service;
    private readonly IConfiguration _config;
    private readonly DiscordSocketClient _botClient;
    private readonly CommandService _commands;
    private readonly ScraperSlashCommands _slash;
    private readonly CrunchyrollSlashCommands _crunchyrollSlashCommands;
    private readonly SlashBuilder _slashBuilder;


    private readonly string PREFIX;
    private readonly string KEY;

    public Bot(IServiceProvider service, IConfiguration config)
	{
        _service = service;
        _config = config;
        _botClient = service.GetRequiredService<DiscordSocketClient>();
        _botClient.PurgeUserCache();
        _commands = service.GetRequiredService<CommandService>();
        _slash = service.GetRequiredService<ScraperSlashCommands>();
        _crunchyrollSlashCommands= service.GetRequiredService<CrunchyrollSlashCommands>();
        _slashBuilder = service.GetRequiredService<SlashBuilder>();

        PREFIX = config["Prefix"];
        KEY = config["DiscordKeys:Token"];
    }

    private void BotEvents()
    {
        _botClient.Log += BotClient_Log;
        _botClient.Ready += BotClient_Ready;
        _botClient.MessageReceived += BotClient_MessageReceived;
        _botClient.SlashCommandExecuted += BotClient_SlashCommandExecuted;
        _botClient.UserJoined += BotClient_UserJoined;
    }

    private async Task BotClient_SlashCommandExecuted(SocketSlashCommand arg)
    {
        if(arg.Data.Name.Equals("scraper"))
        {
            var option = arg.Data.Options.FirstOrDefault();
            switch (option.Name) 
            {
                case "crunchyroll":
                    await _slash.Crunchyroll(arg);
                    break;
                case "imdb":

                    break;
            }
        }
        else if(arg.Data.Name.Equals("crunchyroll"))
        {
            await _crunchyrollSlashCommands.Start(arg);
        }
        else if(arg.Data.Name.Equals("twitch"))
            await _slash.Twitch(arg);
            

}

    private async Task BotClient_UserJoined(SocketGuildUser arg)
    {
        var channel = _botClient.GetChannelAsync(1044857228697554975).Result as IMessageChannel;

        await channel.SendMessageAsync($"Willkommen {arg.Mention}");
    }

    private async Task BotClient_MessageReceived(SocketMessage arg)
    {
        _ = Task.Run(async () =>
        {
            SocketUserMessage message = arg as SocketUserMessage;
            if (message != null)
            {
                int commandPosition = 0;
                if (message.HasStringPrefix(PREFIX, ref commandPosition))
                {
                    SocketCommandContext context = new SocketCommandContext(_botClient, message);
                    var result = await _commands.ExecuteAsync(context, commandPosition, _service);
                    if (!result.IsSuccess)
                    {
                        Log.Logger.Information(result.ErrorReason);
                    }
                }
            }
        });
    }

    private async Task BotClient_Ready()
    {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
        await _botClient.SetGameAsync("Scraping...");

        try
        {
            await _slashBuilder.Start().Result
                   .Scraper().Result
                   .Crunchyroll().Result
                   .Twitch();
        }
        catch (Exception err)
        {

            Console.WriteLine(err.Message);
        }
    }

    private async Task BotClient_Log(LogMessage arg)
    {
        var channel = _botClient.GetChannelAsync(1078987423398236221).Result as IMessageChannel;

        await channel.SendMessageAsync(arg.Message);
        Log.Logger.Information(arg.Message);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {

        BotEvents();

        await _botClient.LoginAsync(TokenType.Bot, KEY);

        await _botClient.StartAsync();

        Console.ReadKey();
        await _botClient.LogoutAsync();
        await _botClient.StopAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _botClient.StopAsync();
    }
}
