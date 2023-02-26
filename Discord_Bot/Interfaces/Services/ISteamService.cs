using Webscraper_API.Scraper.Steam.Models;

namespace Discord_Bot.Interfaces.Services
{
    public interface ISteamService
    {
        Task CreateOrUpdate(Game game);
        Task<Game> GetSteamGame(string url);
    }
}