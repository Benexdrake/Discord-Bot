using System.Net.Http.Json;
using Scraper_Bot.Interfaces.Services;

namespace Scraper_Bot.Services;

public class PokemonService : IPokemonService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _config;
    public PokemonService(IServiceProvider service, IConfiguration config)
    {
        _client = service.GetRequiredService<HttpClient>();
        _config = config;
    }

    public async Task CreateOrUpdateAsync(Pokemon[] pokemons)
    {
        var response = _client.PostAsJsonAsync(_config["API"] + "/api/Pokemon", pokemons).Result.Content.ReadAsStringAsync();
        Console.WriteLine(response.Result);
    }
    public async Task CreateOrUpdateAsync(PokemonCard card)
    {
        var response = _client.PostAsJsonAsync(_config["API"] + "/api/PokemonCard", card).Result.Content.ReadAsStringAsync();
        Console.WriteLine(response.Result);
    }
}