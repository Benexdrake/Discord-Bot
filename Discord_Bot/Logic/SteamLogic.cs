﻿using Discord_Bot.Embeds;
using Discord_Bot.Interfaces.Services;
using System;
using System.Text.RegularExpressions;
using Webscraper_API.Scraper.Steam.Models;
using static System.Net.WebRequestMethods;

namespace Discord_Bot.Logic;

public class SteamLogic
{
    private readonly ISteamService _ss;
    private readonly ISteam_Api _api;
    private readonly SteamEmbed _embed;
    public SteamLogic(IServiceProvider service)
    {
        _ss = service.GetRequiredService<ISteamService>();
        _api = service.GetRequiredService<ISteam_Api>();
        _embed = service.GetRequiredService<SteamEmbed>();
    }

    public async Task GamePerUrl(Discord.IUserMessage message, string url)
    {
        if (!string.IsNullOrWhiteSpace(url))
        {
            message.ModifyAsync(x => x.Content = $"Please wait, looking for the Game: {url}");
            await GetGame(message,url, true);
            
        }
        else
            message.ModifyAsync(x => x.Content = "Something was wrong with the Url");
    }

    public async Task GamesFromWishlist(Discord.IUserMessage message, string wishlistUrl)
    {
        var urls = _api.GetGameUrlsFromWishlist(wishlistUrl);

        while(!urls.IsCompleted)
        {
            message.ModifyAsync(x => x.Content = $"{_api.Message}");
            await Task.Delay(1000);
        }
        int i = 0;
        foreach (var url in urls.Result)
        {
            message.ModifyAsync(x => x.Content = $"Please wait, looking for the Game: {url} - {Helper.Percent(i,urls.Result.Length)} % / 100%");
            await GetGame(message,url, true);
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
            await GetGame(message, url,true);
            i++;
        }
        message.ModifyAsync(x => x.Content = $"Found {urls.Result.Length} Games");
    }

    public async Task GamesUpdate(Discord.IUserMessage message, string category)
    {
        var apps = _api.GetAllGameIds().Result;

        int i = 1;
        
        bool update = false;

        if(category == "hard")
            update = true;

        var games = _ss.GetAllSteamGames().Result.ToList();

        foreach (var app in apps)
        {
            if(!string.IsNullOrWhiteSpace(app.name))
            {
                var dbGame = games.Where(x => x._id.Contains(app.appid.ToString())).FirstOrDefault();
                if(dbGame is null || (dbGame is not null && update))
                {
                    var url = "https://store.steampowered.com/app/" + app.appid;
                    await message.ModifyAsync(x => x.Content = $"Looking for {app.name}\n{Helper.Percent(i, apps.Length)}% / 100%\n{i} of {apps.Length}\n{url}");
                    await GetGame(message, url, update);
                }
            }    
            i++;
        }
    }

    public async Task GetUser(Discord.IUserMessage message, string url)
    {
        message.ModifyAsync(x => x.Content = $"Looking for the User {url}");
        var user = _api.GetUser(url).Result;
        var embed = _embed.UserEmbed(user).Result;

        message.ModifyAsync(x => x.Content = $"Found {user.Username}");
        await message.ModifyAsync(x => x.Embed = embed.Build());

        await _ss.CreateOrUpdateUser(user);
    }

    #region Private
    private async Task GetGame(Discord.IUserMessage message,string url, bool update)
    {
        var game = _api.GetSteamGame(url).Result;
        if(game is not null)
        {
            await _ss.CreateOrUpdate(game);
            await message.ModifyAsync(x => x.Content = $"Found: {game.Title}");
            if(!game.IsDLC)
            {
                var embed = _embed.GameEmbed(game).Result;
                await message.ModifyAsync(x => x.Embed = embed.Build());
            }
        }
    }
    #endregion
}
