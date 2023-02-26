using Webscraper_API.Scraper.Insight_Digital_Handy.Models;

namespace Discord_Bot.Interfaces.Services
{
    public interface IInsightDigitalService
    {
        Task CreateOrUpdateAsync(Handy handy);
        Task<Handy> GetPhoneByIdAsync(string id);
        Task<Handy[]> GetPhonesAsync();
    }
}