using Discord;
using Microsoft.Extensions.Logging;

namespace Discord_Bot_Console.Modules
{
    public class DiscordBotTest : IHostedService
    {
        private readonly DiscordSocketClient _botClient;
        //private readonly CommandService _commands;

        private readonly IServiceProvider _service;
        private readonly ILogger<DiscordBot> _log;
        private readonly IKeys keys;
        private readonly string Prefix;
        private readonly InteractionService _commands;

        public DiscordBotTest(IServiceProvider service, IConfiguration config, ILogger<DiscordBot> log, InteractionService commands)
        {
            keys = service.GetService<IKeys>();
            _service = service;
            _log = log;
            Prefix = config.GetValue<string>("Prefix");
            _botClient = service.GetRequiredService<DiscordSocketClient>();
            //_commands= service.GetRequiredService<CommandService>();
            _commands = commands;
        }

        private void BotEvents()
        {
            _botClient.Log += BotClient_Log;
            _botClient.Ready += BotClient_Ready;
            _botClient.MessageReceived += BotClient_MessageReceived;
            _commands.SlashCommandExecuted += _commands_SlashCommandExecuted;
        }

        private async Task _commands_SlashCommandExecuted(SlashCommandInfo arg1, IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            
        }

        private async Task BotClient_Ready()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
            await _botClient.SetGameAsync("Listening...");
        }

        private Task BotClient_Log(LogMessage arg)
        {
            _log.LogInformation(arg.Message);
            return Task.CompletedTask;
        }

        private async Task BotClient_MessageReceived(SocketMessage arg)
        {
            //_ = Task.Run(async () =>
            //{
            //    SocketUserMessage message = arg as SocketUserMessage;

            //    int commandPosition = 0;
            //    if (message.HasStringPrefix(Prefix, ref commandPosition))
            //    {
            //        SocketCommandContext context = new SocketCommandContext(_botClient, message);
            //        var result = await _commands.ExecuteAsync(context, commandPosition, _service);
            //        if (!result.IsSuccess)
            //        {
            //            _log.LogError(result.ErrorReason);
            //        }
            //    }
            //});
            //return Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            
            BotEvents();
            await _botClient.LoginAsync(TokenType.Bot, keys.Token);
            await _botClient.StartAsync();

            while (!cancellationToken.IsCancellationRequested)
            {

            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _botClient.StopAsync();
        }


    }
}
