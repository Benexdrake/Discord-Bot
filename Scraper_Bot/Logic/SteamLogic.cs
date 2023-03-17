using OpenQA.Selenium.DevTools.V105.Network;
using Scraper_Bot.Interfaces.Services;

namespace Scraper_Bot.Logic;

public class SteamLogic
{
    private readonly ISteamService _ss;
    private readonly ISteam_Api _api;
    public SteamLogic(IServiceProvider service)
    {
        _ss = service.GetRequiredService<ISteamService>();
        _api = service.GetRequiredService<ISteam_Api>();
    }

    public async Task GamePerUrl(Discord.IUserMessage message, string url)
    {
        if (!string.IsNullOrWhiteSpace(url))
        {
            message.ModifyAsync(x => x.Content = $"Please wait, looking for the Game: {url}");
            await GetGame(message, url, true);

        }
        else
            message.ModifyAsync(x => x.Content = "Something was wrong with the Url");
    }

    public async Task GamesFromWishlist(Discord.IUserMessage message, string wishlistUrl)
    {
        var urls = _api.GetGameUrlsFromWishlist(wishlistUrl);

        while (!urls.IsCompleted)
        {
            message.ModifyAsync(x => x.Content = $"{_api.Message}");
            await Task.Delay(1000);
        }
        int i = 0;
        foreach (var url in urls.Result)
        {
            message.ModifyAsync(x => x.Content = $"Please wait, looking for the Game: {url} - {Helper.Percent(i, urls.Result.Length)} % / 100%");
            await GetGame(message, url, true);
            i++;
        }
        message.ModifyAsync(x => x.Content = $"Found {urls.Result.Length} Games");
    }

    public async Task GamesFromCategory(Discord.IUserMessage message, string category)
    {
        int n = int.Parse(category);

        var urls = _api.GetGameUrls(n);
        while (!urls.IsCompleted)
        {
            message.ModifyAsync(x => x.Content = $"{_api.Message}");
            await Task.Delay(1000);
        }
        int i = 0;
        foreach (var url in urls.Result)
        {
            message.ModifyAsync(x => x.Content = $"Please wait, looking for the Game: {url} - {Helper.Percent(i, urls.Result.Length)}% / 100%");
            await GetGame(message, url, true);
            i++;
        }
        message.ModifyAsync(x => x.Content = $"Found {urls.Result.Length} Games");
    }

    public async Task GamesUpdate(Discord.IUserMessage message, string category)
    {
        var apps = _api.GetAllGameIds().Result;

        int i = 1;

        bool update = false;

        if (category == "hard")
            update = true;

        var games = _ss.GetAllSteamGames().Result.ToList();

        List<string> urlList = new List<string>();

        if (File.Exists("urlList.txt"))
            urlList = File.ReadAllLines("urlList.txt").ToList();
        else
            File.Create("urlList.txt");

        foreach (var app in apps)
        {
            var url = "https://store.steampowered.com/app/" + app.appid;

            var dbGame = games.Where(x => x.Url.Contains(url)).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(app.name))
            {
                var u = urlList.Where(x => x.Equals(url)).FirstOrDefault();
                if (u is null)
                    urlList.Add(url);
                if (u is null && dbGame is null || (!update && dbGame is null))
                {
                    await message.ModifyAsync(x => x.Content = $"Looking for {app.name}\n{Helper.Percent(i, apps.Length)}% / 100%\n{i} of {apps.Length-games.Count}\n{url}");
                    await GetGame(message, url, update);       
                }
                File.WriteAllLines("urlList.txt", urlList.ToArray());
            }
            i++;
        }
    }

    public async Task GetUser(Discord.IUserMessage message, string url)
    {
        message.ModifyAsync(x => x.Content = $"Looking for the User {url}");
        var user = _api.GetUser(url).Result;

        message.ModifyAsync(x => x.Content = $"Found {user.Username}");

        await _ss.CreateOrUpdateUser(user);
    }

    #region Private
    private async Task GetGame(Discord.IUserMessage message, string url, bool update)
    {
        var game = _api.GetSteamGame(url).Result;
        if (game is not null)
        {
            await _ss.CreateOrUpdate(game);
            await message.ModifyAsync(x => x.Content = $"Found: {game.Title}");
        }
    }
    #endregion
}