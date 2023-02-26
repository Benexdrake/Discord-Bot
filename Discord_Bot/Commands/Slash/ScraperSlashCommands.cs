using Discord;
using Discord.Interactions;
using Serilog;

namespace Discord_Bot.Commands.Slash;

public class ScraperSlashCommands
{
    private readonly CrunchyrollLogic _cl;
    private readonly TwitchLogic _tl;
	public ScraperSlashCommands(IServiceProvider service)
	{
        _cl = service.GetRequiredService<CrunchyrollLogic>();
        _tl = service.GetRequiredService<TwitchLogic>();
    }

    // Crunchyroll
    public async Task Crunchyroll(SocketSlashCommand arg)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await arg.RespondAsync($"Please wait: {arg.Data.Options.FirstOrDefault().Name}");
                var message = arg.GetOriginalResponseAsync().Result as IUserMessage;

                var option = arg.Data.Options.FirstOrDefault().Options.FirstOrDefault();


                string value = option.Value.ToString();

                switch (value)
                {
                    case "1":
                        await _cl.FullUpdate(message);
                        break;
                    case "2":
                        await _cl.WeeklyUpate(message);
                        break;
                    case "3":
                        await _cl.DailyUpate(message);
                        break;
                    default:
                        await _cl.GetSingleAnime(message, value);
                        break;
                }
            }
            catch (Exception err)
            {
                Log.Logger.Error(err.ToString());
            }
        });
    }

    // IMDb movie by url, favorit list url
    public async Task IMDb(SocketSlashCommand arg)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await arg.RespondAsync("Please wait");
                var message = arg.GetOriginalResponseAsync().Result as IUserMessage;

                var option = arg.Data.Options.FirstOrDefault();

                

                switch ("")
                {

                }
            }
            catch (Exception err)
            {
                Log.Logger.Error(err.ToString());
            }
        });
    }

    // Twitch

    public async Task Twitch(SocketSlashCommand arg)
    {
        _ = Task.Run(async () =>
        {
            await arg.RespondAsync("Please wait");
            var message = arg.GetOriginalResponseAsync().Result as IUserMessage;

            var option = arg.Data.Options.FirstOrDefault();
            await _tl.GetTwitchProfil(message, option.Value.ToString());
        });
    }

    // Steam game by url, top 4 lists

    // Insight Digital handy by url, toplist
}
