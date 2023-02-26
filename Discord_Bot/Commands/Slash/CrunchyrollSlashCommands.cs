using Discord;
using Discord_Bot.Embeds;

namespace Discord_Bot.Commands.Slash;
public class CrunchyrollSlashCommands
{
    private readonly ICrunchyrollService _cs;
    private readonly CrunchyrollEmbed _embed;

    private readonly Random _rand;
    public CrunchyrollSlashCommands(IServiceProvider service)
    {
        _cs = service.GetRequiredService<ICrunchyrollService>();
        _embed = service.GetRequiredService<CrunchyrollEmbed>();

        _rand = new Random();
    }

    public async Task Start(SocketSlashCommand arg)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                Anime anime = new Anime();
                var animes = new List<Anime>();
                await arg.RespondAsync("Please wait");
                var message = arg.GetOriginalResponseAsync().Result as IUserMessage;

                var option = arg.Data.Options.FirstOrDefault();
                string value = option.Value.ToString();

                switch (option.Name)
                {
                    case "name":
                        animes = _cs.GetAnimesByNameAsync(value).Result.ToList();
                        anime = animes[_rand.Next(0, animes.Count)];
                        break;
                    case "url":
                        anime = _cs.GetAnimeByIdAsync(value).Result;
                        break;
                    case "rating":
                        if (double.TryParse(value, out double doublenumber))
                        {
                            animes = _cs.GetAnimesByRatingAsync(doublenumber).Result.ToList();
                            anime = animes[_rand.Next(0, animes.Count)];
                        }
                        break;
                    case "episodes":
                        if(int.TryParse(value, out int intnumber))
                        {
                            animes = _cs.GetAnimesByEpisodesAsync(intnumber).Result.ToList();
                            anime = animes[_rand.Next(0, animes.Count)];
                        }
                        break;
                    case "publisher":
                        animes = _cs.GetAnimeByPublisherAsync(value).Result.ToList();
                        anime = animes[_rand.Next(0, animes.Count)];
                        break;
                    case "genre":
                        animes = _cs.GetAnimesByGenreAsync(value).Result.ToList();
                        anime = animes[_rand.Next(0, animes.Count)];
                        break;
                }
                if (anime.Name is not "")
                {
                    var embed = _embed.AnimeEmbed(anime).Result;
                    await message.ModifyAsync(x => x.Content = $"Anime from Crunchyroll: {anime.Name}");
                    await message.ModifyAsync(x => x.Embed = embed.Build());

                }
                else
                    await message.ModifyAsync(x => x.Content = "Something went wrong, sry");
            }
            catch (Exception err)
            {
                Log.Logger.Error(err.Message);
            }

        });
    }
}
