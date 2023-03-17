using System.Net.Http.Json;
using Scraper_Bot.Interfaces.Services;
using Webscraper_API.Scraper.Crunchyroll.Models;

namespace Scraper_Bot.Services;
public class CrunchyrollService : ICrunchyrollService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _config;

    public CrunchyrollService(IServiceProvider service, IConfiguration config)
    {
        _client = service.GetRequiredService<HttpClient>();
        _config = config;
    }

    public async Task<Anime[]> GetAnimesAsync()
    {
        var animes = _client.GetFromJsonAsync<Anime[]>(_config["API"] + "/api/Crunchyroll").Result;
        return animes;
    }

    public async Task<Anime> GetAnimeByIdAsync(string id)
    {
        var anime = await _client.GetFromJsonAsync<Anime>(_config["API"] + "/api/Crunchyroll/" + id);
        return anime;
    }

    public async Task<Anime> GetRandomAnimeAsync()
    {
        var anime = await _client.GetFromJsonAsync<Anime>(_config["API"] + "/api/Crunchyroll/animerandom");
        return anime;
    }

    public async Task<Anime[]> GetAnimesByNameAsync(string param)
    {
        var anime = await _client.GetFromJsonAsync<Anime[]>(_config["API"] + "/api/Crunchyroll/animesbyname?name=" + param);
        return anime;
    }

    public async Task<Anime[]> GetAnimesByGenreAsync(string genre)
    {
        var anime = await _client.GetFromJsonAsync<Anime[]>(_config["API"] + "/api/Crunchyroll/animesbygenre?tags=" + genre);
        return anime;
    }

    public async Task<Anime[]> GetAnimeByPublisherAsync(string publisher)
    {
        var anime = await _client.GetFromJsonAsync<Anime[]>(_config["API"] + "/api/Crunchyroll/animesbypublisher?publisher=" + publisher);
        return anime;
    }
    public async Task<Anime[]> GetAnimesByEpisodesAsync(int episodes)
    {
        var anime = await _client.GetFromJsonAsync<Anime[]>(_config["API"] + "/api/Crunchyroll/animesbyepisodes?episodes=" + episodes);
        return anime;
    }
    public async Task<Anime[]> GetAnimesByRatingAsync(double rating)
    {
        var anime = await _client.GetFromJsonAsync<Anime[]>(_config["API"] + "/api/Crunchyroll/animesbyrating?rating=" + rating);
        return anime;
    }
    public async Task<Episode[]> GetEpisodesByAnimeIdAsync(string animeId)
    {
        var episodes = await _client.GetFromJsonAsync<Episode[]>(_config["API"] + "/api/Crunchyroll/episodes?animeId=" + animeId);
        return episodes;
    }

    public async Task<Anime_Episodes[]> GetAllAnimeEpisodes()
    {
        var AEs = _client.GetFromJsonAsync<Anime_Episodes[]>(_config["API"] + "/api/Crunchyroll/all").Result;
        return AEs;
    }

    public async Task CreateOrUpdateAsync(Anime_Episodes AE)
    {
        var response = await _client.PostAsJsonAsync(_config["API"] + "/api/Crunchyroll", AE);
    }
}
