using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text;
using Webscraper_API.Scraper.Steam.Models;

namespace Discord_Bot.Services;

public class SteamService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _config;

    private readonly string URL;

    public SteamService(IServiceProvider service, IConfiguration config)
    {
        _client = service.GetRequiredService<HttpClient>();
        _config = config;
        URL = _config["API"] + "/api/steam/";
    }

    public async Task<Game> GetSteamGame(string url)
    {
        var split = url.Split('/');
        var u = URL + split[4];
        var game = _client.GetFromJsonAsync<Game>(u).Result;
        return game;
    }

    public async Task CreateOrUpdate(Game game)
    {
        var result = await _client.PostAsJsonAsync<Game>(URL, game).Result.Content.ReadAsStringAsync();
        Console.WriteLine(result);
    }
}
