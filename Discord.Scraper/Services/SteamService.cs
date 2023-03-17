namespace Scraper_Bot.Services;

public class SteamService : ISteamService
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

    public async Task<SteamGame> GetSteamGame(string url)
    {
        var split = url.Split('/');
        var u = URL + split[4];
        var game = _client.GetFromJsonAsync<SteamGame>(u).Result;
        return game;
    }

    public async Task<SteamGame[]> GetAllSteamGames()
    {
        var games = _client.GetFromJsonAsync<SteamGame[]>(URL).Result;
        return games;
    }

    public async Task CreateOrUpdate(SteamGame game)
    {
        await _client.PostAsJsonAsync<SteamGame>(URL, game);
    }

    public async Task CreateOrUpdateUser(SteamUser user)
    {
        await _client.PostAsJsonAsync<SteamUser>(URL + "user", user);
    }
}