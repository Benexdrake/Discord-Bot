using Discord;
using Webscraper_API;

namespace Discord_Bot.Logic;

public class PokemonLogic
{
	public int MaxPokemons { get; set; }
	private readonly IPokemon_API _PDexApi;
	private readonly PokemonService _ps;
	public PokemonLogic(IServiceProvider service)
	{
		MaxPokemons = 1008;
		_PDexApi = service.GetRequiredService<IPokemon_API>();
		_ps = service.GetRequiredService<PokemonService>();
	}

	public async Task GetPokedex(IUserMessage message)
	{
		await message.ModifyAsync(x => x.Content = "Looking now for Pokemon, please wait");

		for (int i = 1; i <= MaxPokemons; i++)
		{
			var pokemons = _PDexApi.GetPokemonByIDAsync(i).Result;
			if(pokemons is not null)
			{
				if(pokemons.Count > 0)
				{
				    await message.ModifyAsync(x => x.Content = $"Found: {pokemons[0].Name}\n{Helper.Percent(i,MaxPokemons)}% / 100%");
					await _ps.CreateOrUpdateAsync(pokemons.ToArray());
				}
			}
			else
                await message.ModifyAsync(x => x.Content = "Something went wrong");
        }
        await message.ModifyAsync(x => x.Content = "100% / 100%");
    }
}
