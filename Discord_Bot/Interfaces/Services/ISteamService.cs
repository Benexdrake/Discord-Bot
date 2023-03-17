using Webscraper_API.Scraper.Steam.Models;

namespace Discord_Bot.Interfaces.Services
{
    public interface ISteamService
    {
        Task<Game[]> GetAllSteamGames();
        Task CreateOrUpdate(Game game);
        Task CreateOrUpdateUser(User user);
        Task<Game> GetSteamGame(string url);
    }
}