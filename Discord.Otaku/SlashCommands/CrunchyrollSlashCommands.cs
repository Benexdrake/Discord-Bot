namespace Otaku_Bot.SlashCommands;
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
                await arg.RespondAsync("Please wait");

                string name = string.Empty;
                double rating = 0;
                int episode = 0;
                string publisher = string.Empty;
                string genre = string.Empty;


                foreach (var o in arg.Data.Options)
                {
                    if (o.Name.Equals("name"))
                        name = o.Value.ToString();
                    else if (o.Name.Equals("rating"))
                    {
                        var isnumber = double.TryParse(o.Value.ToString(), out double number);
                        if (isnumber) rating = number;
                    }
                    else if (o.Name.Equals("episodes"))
                    {
                        var isnumber = int.TryParse(o.Value.ToString(), out int number);
                        if (isnumber) episode = number;
                    }
                    else if (o.Name.Equals("publisher"))
                        publisher = o.Value.ToString();
                    else if (o.Name.Equals("genre"))
                        genre = o.Value.ToString();
                }

                var animes = new List<Anime>();
                var animeList = _cs.GetAnimesAsync().Result.ToList();


                if(!string.IsNullOrWhiteSpace(name))
                    animeList = animeList.Where(a => a.Name.ToLower().Contains(name.ToLower())).ToList();
                if (!string.IsNullOrWhiteSpace(publisher))
                    animeList = animeList.Where(a => a.Publisher.ToLower().Contains(publisher.ToLower())).ToList();
                if (rating > 0)
                    animeList = animeList.Where(a => a.Rating <= rating).ToList();
                if (episode > 0)
                    animeList = animeList.Where(a => a.Episodes <= episode).ToList();
                if (!string.IsNullOrWhiteSpace(genre))
                    animeList = animeList.Where(a => a.Tags.ToLower().Contains(genre.ToLower() )).ToList();


                var message = arg.GetOriginalResponseAsync().Result as IUserMessage;
                if(animeList.Count > 0)
                {
                    var anime = animeList[_rand.Next(0, animeList.Count)];
                    var embed = _embed.AnimeEmbed(anime).Result;
                    await message.ModifyAsync(x => x.Content = arg.User.Mention);
                    var channel = message.Channel;
                    await channel.SendMessageAsync(embed: embed.Build());
                }
                else
                    await message.ModifyAsync(x => x.Content = "Hey " + arg.User.Mention + ", cant find this Anime, sry");
            }
            catch (Exception err)
            {
                Log.Logger.Error(err.Message);
            }

        });
    }
}
