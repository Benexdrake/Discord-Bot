namespace Scraper_Bot.Interfaces.Services;

public interface ISteamService
{
    Task<SteamGame[]> GetAllSteamGames();
    Task CreateOrUpdate(SteamGame game);
    Task CreateOrUpdateUser(SteamUser user);
    Task<SteamGame> GetSteamGame(string url);
}