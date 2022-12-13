using Discord.Rest;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webscraper_API;

namespace Discord_Bot_Console.D
{
    public class CrunchyrollModule : ModuleBase<SocketCommandContext>
    {
        private readonly Random rand;
        private readonly DiscordEmbedBuilder _discordEmbedBuilder;

        private Browser _browser;
        private CrunchyrollDBContext _context;
        private ICR_API _api;

        public CrunchyrollModule(IServiceProvider service)
        {
            _browser = service.GetService<Browser>();
            _context = service.GetService<CrunchyrollDBContext>();
            _api = service.GetService<ICR_API>();
            _discordEmbedBuilder = service.GetService<DiscordEmbedBuilder>();
            rand = new Random();
        }

        [Command("anime", true)]
        public async Task AnimeByName([Remainder] string param)
        {
            var message = await Context.Message.ReplyAsync($"Looking for {param}");
            var anime = _context.Animes.ToList().Where(x => x.Name.ToLower().Contains(param)).FirstOrDefault();

            if (anime is not null)
            {
                await LoadAnime(anime,message);
            }
        }

        [Command("anime")]
        public async Task AnimeRandom()
        {
            var anime = _context.Animes.ToList().ElementAt(rand.Next(_context.Animes.Count()));

            if (anime is not null)
            {
                var message = await Context.Message.ReplyAsync($"Looking for {anime.Name}");
                
                await LoadAnime(anime,message);
            }
        }

        [Command("FullUpdate")]
        public async Task GetUrls()
        {
            var message = await Context.Message.ReplyAsync("Please Wait, catching Urls from Crunchyroll...");

            var urls = _api.GetAnimeUrlList(_browser.WebDriver).Result;
            await message.ModifyAsync(x => x.Content = $"Found {urls.Length} Animes");

            for (int i = 0; i < urls.Length; i++)
            {
                var a = _context.Animes.Where(x => x.Url.Equals(urls[i])).FirstOrDefault();
                if(a is null) 
                {
                    await GetAnimeByUrl(urls[i], message);
                }
            }
        }

        [Command("GetAllAnimes")]
        public async Task GetAllAnimes()
        {
            var message = await Context.Message.ReplyAsync("Please Wait, looking for Animes from Crunchyroll...");
            var animes = _context.Animes.OrderBy(x => x.Name).ToList();
            foreach (var anime in animes)
            {
                var embed = _discordEmbedBuilder.AnimeEmbed(anime).Result;
                await message.ModifyAsync(x => x.Embed = embed.Build());
                await Task.Delay(1000);
            }
        }

        [Command("GetAnimeByUrl")]
        public async Task GetAnimeByUrl(string url, IUserMessage message)
        {
            
            var doc = _browser.GetPageDocument(url, 1000).Result;
            var anime = _api.GetAnimeByUrlAsync(url, doc).Result;
            
            if (anime is not null)
            {
                await LoadAnime(anime, message);
            }
        }
            
        public async Task DeleteMessages(int n)
        {
            var messages = Context.Channel.GetMessagesAsync(n).Flatten();
            foreach (var h in await messages.ToArrayAsync())
            {
                await Context.Channel.DeleteMessageAsync(h);
            }
        }

        private async Task LoadAnime(Anime anime, IUserMessage message)
        {
            var a = _context.Animes.Where(x => x.Id.Equals(anime.Id)).FirstOrDefault();
            var embed = _discordEmbedBuilder.AnimeEmbed(anime).Result;
            
            await message.ModifyAsync(x => x.Content = $"Found {anime.Name}");
            await message.ModifyAsync(x => x.Embed = embed.Build());
            if(a is null)
            {
                _context.Animes.Add(anime);
                _context.SaveChanges();
            }
            
        }
    }
}
