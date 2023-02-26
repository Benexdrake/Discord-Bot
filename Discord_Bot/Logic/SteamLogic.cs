using Discord;
using Discord_Bot.Interfaces.Services;
using System.Text.RegularExpressions;

namespace Discord_Bot.Logic;

public class SteamLogic
{
    private readonly ISteamService _ss;
    private readonly ISteam_Api _api;
    public SteamLogic(IServiceProvider service)
    {
        _ss = service.GetRequiredService<ISteamService>();
        _api = service.GetRequiredService<ISteam_Api>();
    }

    public async Task GamePerUrl(IUserMessage message, string param)
    {
        var split = param.ToLower().Split(' ');
        string url = string.Empty;
        foreach (var item in split)
        {
            if (item.Contains("https://store.steampowered.com/app/"))
                url = item;
        }
        if (!string.IsNullOrWhiteSpace(url))
        {
            message.ModifyAsync(x => x.Content = $"Please wait, looking for the Game: {url}");
            await GetGame(message,url);
        }
        else
            message.ModifyAsync(x => x.Content = "Something was wrong with the Url");
    }

    public async Task GamePerCategory(IUserMessage message, string param)
    {
        var r = new Regex("[0-9]");
        var match = r.Match(param);
        bool isNumber = int.TryParse(match.Value, out var number);
        if (isNumber)
        {
            if (number > 0 && number < 5)
            {
                int i = 0;
                message.ModifyAsync(x => x.Content = $"Please wait, you choose {match.Value}");
                var urls = _api.GetGameUrls(number).Result;
                if (urls is not null)
                {
                    foreach (var url in urls)
                    {
                        var found = await GetGame(message, url);
                        if (found)
                            i++;
                    }
                    message.ModifyAsync(x => x.Content = $"Found {i} Games");
                }
                else
                    message.ModifyAsync(x => x.Content = "Something went wrong");
            }
            else
                message.ModifyAsync(x => x.Content = "Please Choose between 1-4");
        }
        else
            message.ModifyAsync(x => x.Content = "Please Choose between 1-4");
    }

    #region Private
    private async Task<bool> GetGame(IUserMessage message,string url)
    {
        var foundGame = false;
        var game = _api.GetSteamGame(url).Result;
        if (game is not null)
        {
            await _ss.CreateOrUpdate(game);
            await message.ModifyAsync(x => x.Content = $"Found: {game.Id} - {game.Title}");
            foundGame = true;
        }
        else
            await message.ModifyAsync(x => x.Content = "Something went wrong, cant find the Game");
        return foundGame;
    }
    #endregion
}
