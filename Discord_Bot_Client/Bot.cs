using Discord_Bot_Client.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_Client
{
    public class Bot : IHostedService
    {
        private readonly IServiceProvider _service;
        private readonly IConfiguration _config;

        private readonly DiscordSocketClient _botClient;
        private readonly CommandService _commands;
        private readonly InteractionService _interactioCommands;

        public Bot(IServiceProvider service, IConfiguration config)
        {
            _service = service;
            _botClient = service.GetRequiredService<DiscordSocketClient>();
            _config = service.GetRequiredService<IConfiguration>();
            _commands= service.GetRequiredService<CommandService>();
            _interactioCommands = service.GetRequiredService<InteractionService>();
        }

        private void BotEvents()
        {
            _botClient.Log += BotClient_Log;
            _botClient.Ready += BotClient_Ready;
            _botClient.MessageReceived += BotClient_MessageReceived;
            _botClient.SlashCommandExecuted += BotClient_SlashCommandExecuted;
            
        }

        private async Task BotClient_Disconnected(Exception arg)
        {
            await _botClient.LoginAsync(TokenType.Bot, _config["DiscordKeys:Token"]);
        }

        private async Task BotClient_SlashCommandExecuted(SocketSlashCommand arg)
        {
            arg.RespondAsync($"Looking for {arg.Data.Options.First().Name}: {arg.Data.Options.First().Value}");   
        }

        private async Task BotClient_MessageReceived(SocketMessage arg)
        {
            SocketUserMessage message = arg as SocketUserMessage;
            int commandPosition = 0;
            if (message.HasStringPrefix("!", ref commandPosition))
            {
                SocketCommandContext context = new SocketCommandContext(_botClient, message);
                var result = await _commands.ExecuteAsync(context, commandPosition, _service);
                if (!result.IsSuccess)
                {
                    Log.Error(result.ErrorReason);
                }
            }
        }
        private async Task BotClient_Ready()
        {
            //await _service.GetRequiredService<SlashCommandsBuilder>().Initialization();
            
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
            await _botClient.SetGameAsync("Scraping...");
        }

        private async Task BotClient_Log(LogMessage arg)
        {
            Log.Information(arg.Message);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            BotEvents();
            await _botClient.LoginAsync(TokenType.Bot, _config["DiscordKeys:Token"]);
            await _botClient.StartAsync();


            await Task.Delay(-1);

            await _botClient.LogoutAsync();
            await _botClient.StopAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}
