namespace Discord_Bot_Console.Modules;

public class PokemonModule : ModuleBase<SocketCommandContext>
{
    private readonly PokemonDBContext _context;
    private readonly Browser _browser;
    private readonly IPokemon_API _api;

    private readonly Random rand;
    private readonly DiscordEmbedBuilder _discordEmbedBuilder;

    private readonly int maxPokemons;

    public PokemonModule(IServiceProvider service, IConfiguration configuration)
    {
        _context = service.GetRequiredService<PokemonDBContext>();
        _browser = service.GetRequiredService<Browser>();
        _api = service.GetRequiredService<IPokemon_API>();
        _discordEmbedBuilder = service.GetRequiredService<DiscordEmbedBuilder>();

        rand = new Random();

        maxPokemons = int.Parse(configuration["PokemonMax"]);
    }

    // Get a Pokemon by Nr
    [Command("Pokemon")]
    public async Task GetPokemonByNr(string n)
    {
        var message = await Context.Message.ReplyAsync($"Looking for Pokemon Nr {n}");
        var isNumber = int.TryParse(n, out var number);
        if (isNumber) 
        {
            await GetPokemons(number, message);
        }
    }

    // Get All Pokemon
    [Command("PokemonAll")]
    public async Task GetAllPokemon()
    {
        var message = await Context.Message.ReplyAsync($"Looking for all Pokemons");
        for (int i = 1; i <= maxPokemons; i++)
        {
            await GetPokemons(i, message);
        }   
    }

    // Get a Random Pokemon
    [Command("PokemonRandom")]
    public async Task GetRandomPokemon()
    {
        var message = await Context.Message.ReplyAsync($"Looking for a random Pokemon");
        int randomNumber = rand.Next(1, maxPokemons);
        await GetPokemons(randomNumber,message);
    }

    // Private Get Pokemon
    private async Task GetPokemons(int id, IUserMessage message)
    {
        var p = _context.Pokemons.Where(x => x.Nr.Equals(id)).ToList();
        if (p is null || p.Count == 0)
        {
            string url = $"https://www.pokemon.com/de/pokedex/{id}";
            var doc = _browser.GetPageDocument(url, 3000).Result;
            p = _api.GetPokemonByIDAsync(id.ToString(), doc).Result;

            foreach (var poke in p)
            {
                SavePokemon(poke);
            }
        }
        List<Embed> embeds = new List<Embed>();
        foreach (var pokemon in p)
        {
            await Task.Delay(1000);
            var embed = _discordEmbedBuilder.PokemonEmbed(pokemon).Result;
            embeds.Add(embed.Build());
        }
        await message.ModifyAsync(x => x.Content = $"Found Nr: {p[0].Nr} - {p[0].Name}");
        await message.ModifyAsync(x => x.Embeds = embeds.ToArray());
    }

    // Save Pokemon in DB
    private void SavePokemon(Pokemon pokemon)
    {
        _context.Pokemons.Add(pokemon);
        _context.SaveChanges();
    }

    // Get Pokemon Json
}
