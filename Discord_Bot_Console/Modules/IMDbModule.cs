using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webscraper_API;
using Webscraper_API.Scraper.IMDB.Models;

namespace Discord_Bot_Console.Modules
{
    public class IMDbModule : ModuleBase<SocketCommandContext>
    {
        private readonly ImdbDBContext _context;
        private readonly Browser _browser;
        private readonly IIMDb_API _api;

        private readonly Random rand;
        private readonly DiscordEmbedBuilder _discordEmbedBuilder;
        public IMDbModule(IServiceProvider service)
        {
            _context = service.GetRequiredService<ImdbDBContext>();
            _browser= service.GetRequiredService<Browser>();
            _api = service.GetRequiredService<IIMDb_API>();
            _discordEmbedBuilder = service.GetRequiredService<DiscordEmbedBuilder>();

            rand = new Random();
        }

        // Get a Single Movie with url
        [Command("Movieurl", true)]
        public async Task GetMovieWithUrl([Remainder] string url)
        {
            var message = await Context.Message.ReplyAsync($"Looking for {url}");
            var movie = await GetMovie(url);
            await SendEmbed(movie,message);
        }

        // Get a Single Movie with Name from DB
        [Command("Movie",true)]
        public async Task GetMovieByName(string param)
        {
            var message = await Context.Message.ReplyAsync($"Looking for {param}");
            var movie = _context.Movies.Where(x => x.Title.Contains(param)).FirstOrDefault();
            await SendEmbed(movie,message);
        }
        // Get a Random Movie from DB without param
        [Command("Movie")]
        public async Task GetMovieRandom()
        {
            var message = await Context.Message.ReplyAsync($"Looking for a Random Movie");
            var movie = _context.Movies.ToList().ElementAt(rand.Next(_context.Movies.Count()));
            await SendEmbed(movie, message);
        }

        // Get Top 250 Movies
        [Command("MovieTop250")]
        public async Task GetTop250()
        {
            var message = await Context.Message.ReplyAsync("Looking for Top 250 Movies");
            var doc = _browser.GetPageDocument("https://www.imdb.com/chart/top/?ref_=nv_mv_250", 0).Result;
            var urls = _api.GetMovieTop250Urls(doc).Result;
            message.ModifyAsync(x => x.Content = "Found 250 Movies");
            foreach (var url in urls)
            {
                var movie = await GetMovie(url);
                
                await SendEmbed(movie, message);
            }
            
        }

        // Movie Scrape Methode
        private async Task<Movie> GetMovie(string url)
        {
            // Check if ID from Url exists in DB

            var split = url.Split('/');
            string id = string.Empty;
            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].Contains("title"))
                {
                    id= split[i+1];
                    break;
                }
            }

            url = "https://www.imdb.com/title/" + id;

            var m = _context.Movies.Where(x => x.Id.Equals(id)).FirstOrDefault();
            if (m is null)
            {
                var doc = _browser.GetPageDocument(url, 2000).Result;
                var movie = _api.GetMovieByUrlAsync(url, doc).Result;
                SaveMovie(movie);
                return movie;
            }
            return m;
        }

        // Save Movie in DB
        private void SaveMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }        

        private async Task SendEmbed(Movie movie, IUserMessage message)
        {
            var embed = _discordEmbedBuilder.IMDBEmbed(movie).Result;

            await message.ModifyAsync(x => x.Content = $"Found {movie.Title}");
            await message.ModifyAsync(x => x.Embed = embed.Build());
        }

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
