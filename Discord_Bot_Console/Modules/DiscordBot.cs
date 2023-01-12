using Discord;
using Microsoft.Extensions.Logging;

namespace Discord_Bot_Console.Modules
{
    public class DiscordBot : IHostedService
    {
        private readonly DiscordSocketClient _botClient;
        private readonly CommandService _commands;

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
            _botClient = service.GetRequiredService<DiscordSocketClient>();
            _commands= service.GetRequiredService<CommandService>();
        }

        private void BotEvents()
        {
            _botClient.Log += BotClient_Log;
            _botClient.Ready += BotClient_Ready;
            _botClient.MessageReceived += BotClient_MessageReceived;
            _botClient.SlashCommandExecuted += _botClient_SlashCommandExecuted;
        }

        private async Task _botClient_SlashCommandExecuted(SocketSlashCommand arg)
        {
            switch(arg.CommandName)
            {
                case "hello-world":

                    var mb = new ModalBuilder()
                    .WithTitle("Fav Food")
                    .WithCustomId("food_menu")
                    .AddTextInput("What??", "food_name", placeholder: "Pizza")
                    .AddTextInput("Why??", "food_reason", TextInputStyle.Paragraph,
                        "Kus it's so tasty");


                    await arg.RespondWithModalAsync(mb.Build());
                    break;
                case "test":



                    
                    break;
            }
        }

        private async Task BotClient_Ready()
        {
            //var guild = _botClient.GetGuild(123456789);

            //var guildCommand = new SlashCommandBuilder();
            //guildCommand.Name = "test-123";
            //guildCommand.WithDescription("This is my first guild slash command!");
            //await guild.CreateApplicationCommandAsync(guildCommand.Build());


            //var globalCommand = new SlashCommandBuilder();
            //globalCommand.WithName("test123")
            //    .WithDescription("Test");
            //
            //await _botClient.CreateGlobalApplicationCommandAsync(globalCommand.Build());

            //await _botClient.Rest.CreateGlobalCommand(globalCommand.Build());
            
            
            //var commands = _botClient.GetGlobalApplicationCommandsAsync().Result;

            //foreach (var command in commands)
            //{
            //    await command.DeleteAsync();
            //}



            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
            await _botClient.SetGameAsync("Scraping...");
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
                    SocketCommandContext context = new SocketCommandContext(_botClient, message);
                    var result = await _commands.ExecuteAsync(context, commandPosition, _service);
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
            
            BotEvents();
            await _botClient.LoginAsync(TokenType.Bot, keys.Token);
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
}
