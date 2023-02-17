using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Discord_Bot;

public class Bot : IHostedService
{
    private readonly IServiceProvider _service;
    private readonly IConfiguration _config;
    private readonly DiscordSocketClient _botClient;
    private readonly CommandService _commands;

    private readonly string PREFIX;
    private readonly string KEY;

    public Bot(IServiceProvider service, IConfiguration config)
	{
        _service = service;
        _config = config;
        _botClient = service.GetRequiredService<DiscordSocketClient>();
        _commands = service.GetRequiredService<CommandService>();
        PREFIX = config["Prefix"];
        KEY = config["DiscordKeys:Token"];
    }

    private void BotEvents()
    {
        _botClient.Log += BotClient_Log;
        _botClient.Ready += BotClient_Ready;
        _botClient.MessageReceived += BotClient_MessageReceived;
    }

    private async Task BotClient_MessageReceived(SocketMessage arg)
    {
        _ = Task.Run(async () =>
        {

        SocketUserMessage message = arg as SocketUserMessage;

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
        });
    }

    private async Task BotClient_Ready()
    {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
        await _botClient.SetGameAsync("Scraping...");
    }

    private async Task BotClient_Log(LogMessage arg)
    {
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
