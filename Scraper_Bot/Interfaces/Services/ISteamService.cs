namespace Scraper_Bot.Interfaces.Services;

public interface ISteamService
{
    Task<Game[]> GetAllSteamGames();
    Task CreateOrUpdate(Game game);
    Task CreateOrUpdateUser(User user);
    Task<Game> GetSteamGame(string url);
}