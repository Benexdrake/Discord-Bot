using Discord;

namespace Otaku_Bot;

public class Bot : IHostedService
{
    private readonly IConfiguration _config;
    private readonly CrunchyrollSlashCommands _csc;
    private readonly SlashBuilder _slashBuilder;
    private readonly ICrunchyrollService _cs;
    private readonly CrunchyrollEmbed _embed;

    private readonly DiscordSocketClient _littleOtaku;

    public Bot(IServiceProvider service, IConfiguration config)
    {
        _config = config;
        _csc = service.GetRequiredService<CrunchyrollSlashCommands>();
        _littleOtaku = service.GetRequiredService<DiscordSocketClient>();
        _slashBuilder= service.GetRequiredService<SlashBuilder>();
        _cs = service.GetRequiredService<ICrunchyrollService>();
        _embed = service.GetRequiredService<CrunchyrollEmbed>();
    }

    private void BotEvents()
    {
        _littleOtaku.Log += LittleOtaku_Log;
        _littleOtaku.Ready += LittleOtaku_Ready;
        _littleOtaku.SlashCommandExecuted += LittleOtaku_SlashCommandExecuted;
        _littleOtaku.MessageReceived += LittleOtaku_MessageReceived;
    }

    private async Task LittleOtaku_MessageReceived(SocketMessage arg)
    {
        _ = Task.Run(async () =>
        {
            var message = arg as SocketUserMessage;

            var m = arg.Content.Split(" ");

            string id = string.Empty;

            foreach (var i in m)
            {
                if(i.Contains("https://www.crunchyroll.com/de/series/"))
                {
                    var split = message.Content.Split('/');
                    for (int j   = 0; j < split.Length; j++)
                    {
                        if (split[j].Equals("series"))
                            id = split[j+1];
                    }

                    var anime = _cs.GetAnimeByIdAsync(id).Result;
                    if(!string.IsNullOrWhiteSpace(anime.Name))
                    {
                        var embed = _embed.AnimeEmbed(anime).Result; 
                        await message.ReplyAsync(embed: embed.Build());
                        return;
                    }
                }
            }
        });
    }

    private async Task LittleOtaku_SlashCommandExecuted(SocketSlashCommand arg)
    {
        if (arg.Data.Name.Equals("crunchyroll"))
        {
            await _csc.Start(arg);
        }
    }

    private async Task LittleOtaku_Ready()
    {

        //await _littleOtaku.SetGameAsync("with Gumdams...");
        await _littleOtaku.SetActivityAsync(new Game("der Community", ActivityType.Listening));
        await _slashBuilder.Start().Result.Crunchyroll();
    }

    private async Task LittleOtaku_Log(LogMessage arg)
    {
        Log.Logger.Information(arg.Message);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        BotEvents();
        var key = _config["DiscordKeys:Token"];
        await _littleOtaku.LoginAsync(TokenType.Bot, key);
        await _littleOtaku.StartAsync();

        Console.ReadKey();

        await _littleOtaku.LogoutAsync();
        await _littleOtaku.StopAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        
    }
}
