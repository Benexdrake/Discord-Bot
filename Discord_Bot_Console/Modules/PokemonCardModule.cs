using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_Console.Modules
{
    public class PokemonCardModule : ModuleBase<SocketCommandContext>
    {
        private Browser _browser;
        private readonly DiscordEmbedBuilder _discordEmbedBuilder;

        private readonly ITCG_API _api;
        private readonly PokemonCardDbContext _context;

        private readonly Random rand;


        public PokemonCardModule(IServiceProvider service)
        {
            _browser = service.GetService<Browser>();
            _discordEmbedBuilder = service.GetService<DiscordEmbedBuilder>();

            _api = service.GetRequiredService<ITCG_API>();
            _context = service.GetRequiredService<PokemonCardDbContext>();

            rand = new Random();
        }

        [Command("PokemonCard")]
        public async Task GetPokemonCardByName(string name)
        {
            var p = _context.PokemonCards.Where(x => x.Name.Contains(name)).ToList();
            if(p.Count > 0)
            {
                int n = rand.Next(0, p.Count);
                await Context.Channel.SendMessageAsync(p[n].Name);
            }
        }

        // All PokemonCards Full Update
        [Command("PokemonCardsAll")]
        public async Task GetAllPokemonCards()
        {
            var message = await Context.Message.ReplyAsync("Searching for All Card Urls, please wait...");
            
            string url = $"https://www.pokemon.com/de/pokemon-sammelkartenspiel/pokemon-karten/1?cardName=&cardText=&evolvesFrom=&card-grass=on&card-fire=on&card-water=on&card-lightning=on&card-psychic=on&card-fighting=on&card-darkness=on&card-metal=on&card-colorless=on&card-fairy=on&card-dragon=on&simpleSubmit=&format=unlimited&hitPointsMin=0&hitPointsMax=340&retreatCostMin=0&retreatCostMax=5&totalAttackCostMin=0&totalAttackCostMax=5&particularArtist=&sort=number&sort=number";

            List<string> urlList= new List<string>();
            
            var doc = _browser.GetPageDocument(url,0).Result;
            int max = _api.GetMaxPokemonCards(doc).Result;
            await message.ModifyAsync(x => x.Content = $"Found {max} Sites...");
            await Task.Delay(1000);
            
            for (int i = 1; i < max; i++)
            {
                url = $"https://www.pokemon.com/de/pokemon-sammelkartenspiel/pokemon-karten/{i}?cardName=&cardText=&evolvesFrom=&card-grass=on&card-fire=on&card-water=on&card-lightning=on&card-psychic=on&card-fighting=on&card-darkness=on&card-metal=on&card-colorless=on&card-fairy=on&card-dragon=on&simpleSubmit=&format=unlimited&hitPointsMin=0&hitPointsMax=340&retreatCostMin=0&retreatCostMax=5&totalAttackCostMin=0&totalAttackCostMax=5&particularArtist=&sort=number&sort=number";
                doc = _browser.GetPageDocument(url, 0).Result;
                var urls = _api.GetUrlsFromSite(doc).Result;
                urlList.AddRange(urls);
                await message.ModifyAsync(x => x.Content =$"{Helper.Percent(i,max)}% / 100%");
            }
            await message.ModifyAsync(x => x.Content = $"Found {urlList.Count} urls, looking now for Cards, please wait...");

            foreach (var u in urlList)
            {
                var p = _context.PokemonCards.Where(x => x.Url.Equals(u)).FirstOrDefault();
                if(p is null)
                {
                    doc = _browser.GetPageDocument(u, 0).Result;
                    p = _api.GetPokemonCardAsync(u, doc).Result;
                    SavePokemon(p);
                    await message.ModifyAsync(x => x.Content = $"Found ID: {p.Id} Name: {p.Name} Element: {p.Element}");
                }
            }
        }

        // Single Card Get

        // Save in DB
        private void SavePokemon(PokemonCard pokemon)
        {
            _context.PokemonCards.Add(pokemon);
            _context.SaveChanges();
        }
        // Output Json

        public async Task DeleteMessages(int n)
        {
            var messages = Context.Channel.GetMessagesAsync(n).Flatten();
            foreach (var h in await messages.ToArrayAsync())
            {
                await Context.Channel.DeleteMessageAsync(h);
            }
        }

    }
}
