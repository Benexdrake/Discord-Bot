using Discord;
using Discord.Interactions;
using Scraper_Bot.Logic;
using Serilog;

namespace Scraper_Bot.Commands.Slash;

public class ScraperSlashCommands
{
    private readonly CrunchyrollLogic _cl;
    private readonly SteamLogic _sl;
    public ScraperSlashCommands(IServiceProvider service)
    {
        _cl = service.GetRequiredService<CrunchyrollLogic>();
        _sl = service.GetRequiredService<SteamLogic>();
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

    public async Task Steam(SocketSlashCommand arg)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await arg.RespondAsync("Please wait");
                var message = arg.GetOriginalResponseAsync().Result as IUserMessage;

                var option = arg.Data.Options.FirstOrDefault().Options.FirstOrDefault();


                string value = option.Value.ToString();

                switch (option.Name)
                {
                    case "game":
                        await _sl.GamePerUrl(message, option.Value.ToString());
                        break;
                    case "wishlist":
                        await _sl.GamesFromWishlist(message, option.Value.ToString());
                        break;
                    case "category":
                        await _sl.GamesFromCategory(message, option.Value.ToString());
                        break;
                    case "user":
                        await _sl.GetUser(message, option.Value.ToString());
                        break;
                    case "update":
                        await _sl.GamesUpdate(message, option.Value.ToString());
                        break;
                    default:

                        break;
                }
            }
            catch (Exception err)
            {

                Log.Logger.Error(err.Message);
            }

        });
    }

    // Steam game by url, top 4 lists

    // Insight Digital handy by url, toplist
}
