namespace Scraper_Bot.Interfaces.Services
{
    public interface IPokemonService
    {
        Task CreateOrUpdateAsync(Pokemon[] pokemons);
        Task CreateOrUpdateAsync(PokemonCard card);
    }
}