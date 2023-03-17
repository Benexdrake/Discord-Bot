namespace Scraper_Bot.Services;

public class InsightDigitalService : IInsightDigitalService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _config;

    public InsightDigitalService(IServiceProvider service, IConfiguration config)
    {
        _client = service.GetRequiredService<HttpClient>();
        _config = config;
    }

    public async Task<Handy> GetPhoneByIdAsync(string id)
    {
        var handy = await _client.GetFromJsonAsync<Handy>(_config["API"] + "/api/InsightDigital/" + id);
        return handy;
    }

    public async Task<Handy[]> GetPhonesAsync()
    {
        var handys = await _client.GetFromJsonAsync<Handy[]>(_config["API"] + "/api/InsightDigital");
        return handys;
    }

    public async Task CreateOrUpdateAsync(Handy handy)
    {
        var response = await _client.PostAsJsonAsync(_config["API"] + "/api/InsightDigital", handy).Result.Content.ReadAsStringAsync();
    }
}
