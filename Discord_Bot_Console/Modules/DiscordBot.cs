using Microsoft.Extensions.Logging;

namespace Discord_Bot_Console.D
{
    public class DiscordBot : IHostedService
    {
        DiscordSocketClient botClient;
        CommandService commands;

        private readonly IServiceProvider _service;
        private readonly ILogger<DiscordBot> _log;
        private readonly IKeys keys;
        private readonly string Prefix;

        public DiscordBot(IServiceProvider service, IConfiguration config, ILogger<DiscordBot> log)
        {
            keys = service.GetService<IKeys>();
            _service = service;
            _log = log;
            Prefix = config.GetValue<string>("Prefix");

        }

        private void BotEvents()
        {
            botClient.Log += BotClient_Log;
            botClient.Ready += BotClient_Ready;
            botClient.MessageReceived += BotClient_MessageReceived;
        }

        private async Task BotClient_Ready()
        {
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
            await botClient.SetGameAsync("Listening...");
        }

        private Task BotClient_Log(LogMessage arg)
        {
            _log.LogInformation(arg.Message);
            return Task.CompletedTask;
        }

        private Task BotClient_MessageReceived(SocketMessage arg)
        {
            _ = Task.Run(async () =>
            {
                SocketUserMessage message = arg as SocketUserMessage;

                int commandPosition = 0;
                if (message.HasStringPrefix(Prefix, ref commandPosition))
                {
                    SocketCommandContext context = new SocketCommandContext(botClient, message);
                    var result = await commands.ExecuteAsync(context, commandPosition, _service);
                    if (!result.IsSuccess)
                    {
                        _log.LogError(result.ErrorReason);
                    }
                }
            });
            return Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            commands = new CommandService();
            botClient = new DiscordSocketClient();
            BotEvents();
            await botClient.LoginAsync(TokenType.Bot, keys.Token);
            await botClient.StartAsync();

            while(!cancellationToken.IsCancellationRequested)
            {
                
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await botClient.StopAsync();
        }
    }
}
