using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Discord_Bot.Logic;
using System.Text.RegularExpressions;

namespace Discord_Bot.Commands;
public class ScraperCommands : ModuleBase<SocketCommandContext>
{
    private readonly CrunchyrollLogic _cl;
    private readonly SteamLogic _sl;
    private readonly InsightDigitalLogic _idl;
    private readonly PokemonLogic _pl;
    public ScraperCommands(IServiceProvider service)
    {
        _cl = service.GetRequiredService<CrunchyrollLogic>();
        _sl= service.GetRequiredService<SteamLogic>();
        _idl = service.GetRequiredService<InsightDigitalLogic>();
        _pl = service.GetRequiredService<PokemonLogic>();
    }

    #region Crunchyroll
    [Command("scraper.crunchyroll")]
    [Alias("s.crunchyroll")]
    public async Task Crunchyroll(string param)
    {
        if(Context.User is SocketGuildUser user)
        {
            if (user.Roles.Any(r => r.Name == "Scraper"))
            {
                if (param.ToLower().Contains("fullupate"))
                {
                    var message = Context.Message.ReplyAsync("Please wait, looking for Urls!").Result;
                    await _cl.FullUpdate(message);
                }
                else if (param.ToLower().Contains("weeklyupdate"))
                {
                    var message = Context.Message.ReplyAsync("Please wait, looking for Urls!").Result;
                    await _cl.WeeklyUpate(message);
                }
                else if (param.ToLower().Contains("dailyupdate"))
                {
                    var message = Context.Message.ReplyAsync("Please wait, looking for Urls!").Result;
                    await _cl.DailyUpate(message);
                }
                else
                    await Context.Message.ReplyAsync("Your Parameter was incorrect, please use Fullupdate, Weeklyupdate or DailyUpdate");
            }
            else
                await Context.Message.ReplyAsync("Only User with Scraper Role can access the Scraper Commands...");
        }
    }
    #endregion

    #region IMDb
    [Command("scraper.imdb")]
    [Alias("s.imdb")]
    public async Task IMDb(string param)
    {
        if (Context.User is SocketGuildUser user)
        {
            if (user.Roles.Any(r => r.Name == "Scraper"))
            {
                if (param.ToLower().Contains("top250"))
                {
                    var message = Context.Message.ReplyAsync("Please wait, looking for Urls!").Result;
                    
                }
                else if (param.ToLower().Contains("favorits"))
                {
                    var message = Context.Message.ReplyAsync("Please wait, looking for Urls!").Result;
                    
                }
                else if (param.ToLower().Contains("url"))
                {
                    var message = Context.Message.ReplyAsync("Please wait, looking for Urls!").Result;
                    
                }
                else
                    await Context.Message.ReplyAsync("Your Parameter was incorrect, please use ");
            }
            else
                await Context.Message.ReplyAsync("Only User with Scraper Role can access the Scraper Commands...");
        }
    }
    #endregion

    #region Steam
    [Command("scraper.steam")]
    [Alias("s.steam")]
    public async Task Steam([Remainder] string param)
    {
        var message = Context.Message.ReplyAsync("Please wait").Result;
        if (Context.User is SocketGuildUser user)
        {
            if (user.Roles.Any(r => r.Name == "Scraper"))
            {
                if (param.ToLower().Contains("category"))
                {
                   await _sl.GamePerCategory(message, param);
                }
                else if (param.ToLower().Contains("store.steampowered.com"))
                {
                   await _sl.GamePerUrl(message, param);
                }
                else
                    await Context.Message.ReplyAsync("Your Parameter was incorrect, please use Url or Category 1-4");
            }
            else
                await Context.Message.ReplyAsync("Only User with Scraper Role can access the Scraper Commands...");
        }
    }
    #endregion

    #region Insight Digital Handys
    [Command("scraper.insightdigitalhandy")]
    [Alias("s.insightdigitalhandy")]
    public async Task InsightDigitalHandys([Remainder]string param)
    {
        var message = Context.Message.ReplyAsync("Please wait").Result;
        if (Context.User is SocketGuildUser user)
        {
            if (user.Roles.Any(r => r.Name == "Scraper"))
            {
                if (param.ToLower().Contains("url"))
                {
                    await _idl.GetHandy(message, param);
                }
                else if (param.ToLower().Contains("top"))
                {
                    await _idl.GetHandys(message);
                }
                else
                    await Context.Message.ReplyAsync("Your Parameter was incorrect, please use Url or Top");
            }
            else
                await Context.Message.ReplyAsync("Only User with Scraper Role can access the Scraper Commands...");
        }
    }
    #endregion

    #region Pokemons
    [Command("scraper.pokemon")]
    [Alias("s.pokemon")]
    public async Task Pokemon([Remainder] string param)
    {
        var message = Context.Message.ReplyAsync("Please wait").Result;
        if (Context.User is SocketGuildUser user)
        {
            if (user.Roles.Any(r => r.Name == "Scraper"))
            {
                if (param.ToLower().Contains("pokedex"))
                {
                    await _pl.GetPokedex(message);
                }
                else if (param.ToLower().Contains("cards"))
                {
                    
                }
                else
                    await Context.Message.ReplyAsync("Your Parameter was incorrect, please use Url or Top");
            }
            else
                await Context.Message.ReplyAsync("Only User with Scraper Role can access the Scraper Commands...");
        }
    }
    #endregion

    #region 

    #endregion
}
