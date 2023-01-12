using Discord.Rest;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webscraper_API;

namespace Discord_Bot_Console.Modules
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

        // Neu

        [Command("crunchyroll")]
        public async Task Crunchyroll(string command = "", [Remainder] string param = "")
        {
            var message = await Context.Message.ReplyAsync("Please wait looking for an Anime");
            Anime anime = null;
            bool isNumber;
            switch (command)
            {
                case "":
                    anime = _context.Animes.ToList().ElementAt(rand.Next(_context.Animes.Count()));
                    break;
                case "name":
                    anime = _context.Animes.ToList().Where(x => x.Name.ToLower().Contains(param)).FirstOrDefault();
                    break;
                case "url":
                    anime = _context.Animes.ToList().Where(x => x.Url.Equals(param)).FirstOrDefault();
                    break;
                case "genre":
                    anime = _context.Animes.ToList().Where(x => x.Tags.ToLower().Contains(param)).FirstOrDefault();
                    break;
                case "rating":
                    isNumber = double.TryParse(param, out double d);
                    if (isNumber)
                        anime = _context.Animes.ToList().Where(x => x.Rating >= d).FirstOrDefault();
                    break;
                case "episodes":
                    isNumber = int.TryParse(param, out int n);
                    if (isNumber)
                        anime = _context.Animes.ToList().Where(x => x.Episodes >= n).FirstOrDefault();
                    break;
                case "publisher":
                    anime = _context.Animes.ToList().Where(x => x.Publisher.ToLower().Contains(param)).FirstOrDefault();
                    break;
                case "fullupdate":
                    await FullUpdate(message);
                    break;
                default:
                    await message.ModifyAsync(x => x.Content = "Your Command was False, please use !crunchyroll without Parameter for random or name [name] or url [url] like !crunchyroll name one piece");
                    break;

            }

            if (anime is not null)
            {
                var embed = _discordEmbedBuilder.AnimeEmbed(anime).Result;
                await message.ModifyAsync(x => x.Embed = embed.Build());
            }
            else
                await message.ModifyAsync(x => x.Content = "Cant find what u searching");

        }


        // Private Logic
        private async Task FullUpdate(IUserMessage message)
        {
            await message.ModifyAsync(x => x.Content = "Please Wait, catching Urls from Crunchyroll...");

            var urls = _api.GetAllAnimeUrlsAsync().Result;
            await message.ModifyAsync(x => x.Content = $"Found {urls.Length} Animes");

            for (int i = 0; i < urls.Length; i++)
            {
                var a = _context.Animes.Where(x => x.Url.Equals(urls[i])).FirstOrDefault();
                if (a is null)
                {
                    await GetAnimeByUrl(urls[i], message);
                }
            }
        }

        private async Task GetAnimeByUrl(string url, IUserMessage message)
        {
            var anime = _api.GetAnimeByUrlAsync(url, 2000).Result;

            if (anime is not null)
            {
                await LoadAnime(anime, message);
            }
        }

        private async Task LoadAnime(Anime anime, IUserMessage? message)
        {
            var a = _context.Animes.Where(x => x.Id.Equals(anime.Id)).FirstOrDefault();
            var embed = _discordEmbedBuilder.AnimeEmbed(anime).Result;

            await message.ModifyAsync(x => x.Content = $"Found {anime.Name}");
            await message.ModifyAsync(x => x.Embed = embed.Build());
            if (a is null)
            {
                _context.Animes.Add(anime);
                _context.SaveChanges();
            }

        }
    }
}
