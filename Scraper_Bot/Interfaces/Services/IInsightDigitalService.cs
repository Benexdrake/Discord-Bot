namespace Scraper_Bot.Interfaces.Services
{
    public interface IInsightDigitalService
    {
        Task CreateOrUpdateAsync(Handy handy);
        Task<Handy> GetPhoneByIdAsync(string id);
        Task<Handy[]> GetPhonesAsync();
    }
}