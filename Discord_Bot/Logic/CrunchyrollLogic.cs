using Discord;
using Discord_Bot.Interfaces.Services;

namespace Discord_Bot.Logic;

public class CrunchyrollLogic
{
    private readonly ICR_API _api;
    private readonly ICrunchyrollService _cs;
    public CrunchyrollLogic(IServiceProvider service)
    {
        _api = service.GetRequiredService<ICR_API>();
        _cs = service.GetRequiredService<ICrunchyrollService>();
    }

    public async Task FullUpdate(IUserMessage message)
    {
        var watchUrls = _api.GetAllAnimeUrlsAsync();
        while (!watchUrls.IsCompleted)
        {
            await message.ModifyAsync(x => x.Content = $"{_api.Message}");
            await Task.Delay(1000);
        }
        var urls = watchUrls.Result;
        await GetAnimeWithEpisodes(urls, message);
    }

    public async Task WeeklyUpate(IUserMessage message)
    {
        var watchUrls = _api.GetWeeklyUpdateAsync();
        while (!watchUrls.IsCompleted)
        {
            await message.ModifyAsync(x => x.Content = $"{_api.Message}");
            await Task.Delay(1000);
        }
        var urls = watchUrls.Result;
        await GetAnimeWithEpisodes(urls, message);
    }
    public async Task DailyUpate(IUserMessage message)
    {
        var urls = _api.GetDailyUpdateAsync().Result;
        await GetAnimeWithEpisodes(urls, message);
    }

    public async Task GetSingleAnime(IUserMessage message, string url)
    {
        var urls = new List<string>();
        urls.Add(url);
        await GetAnimeWithEpisodes(urls.ToArray(), message);
    }

    #region Private Logic
    private async Task GetAnimeWithEpisodes(string[] urls, IUserMessage message)
    {
        int i = 1;
        foreach (var url in urls)
        {
            var split = url.Split('/');
            if(urls.Length> 1)
                await message.ModifyAsync(x => x.Content = $"{i}/{urls.Length} Urls\nLooking for {url}\n{Helper.Percent(i, urls.Length)}% / 100%");
            else
                await message.ModifyAsync(x => x.Content = $"Looking for {url}");

            var AE = _api.GetAnimewithEpisodes(url, 2000).Result;
            if (AE is not null)
            {
                var EList = _cs.GetEpisodesByAnimeIdAsync(AE.Anime.Id).Result.ToList();
                for (int n = 0; n < AE.Episodes.Length; n++)
                {
                    var E = EList.Where(x => x.Id.Equals(AE.Episodes[n].Id)).FirstOrDefault();
                    if(E is not null)
                    {
                        if (string.IsNullOrWhiteSpace(E.Description) || string.IsNullOrWhiteSpace(E.ReleaseDate))
                        {
                            if(urls.Length > 1)
                                await message.ModifyAsync(x => x.Content = $"{i}/{urls.Length} Urls\nEpisode Details from {AE.Anime.Name}\n{Helper.Percent(n, AE.Episodes.Length)}% / 100%\n{n + 1}/{AE.Episodes.Length}");
                            else
                                await message.ModifyAsync(x => x.Content = $"Episode Details from {AE.Anime.Name}\n{Helper.Percent(n, AE.Episodes.Length)}% / 100%\n{n + 1}/{AE.Episodes.Length}");
                            var episode = _api.GetEpisodeDetails(AE.Episodes[n]).Result;
                            if (episode is not null)
                            {
                                if (episode.Description == "")
                                    Console.WriteLine($"Empty Description... {episode.AnimeId} - {episode.Url}");
                                episode.EpisodeNr = AE.Episodes[n].EpisodeNr;
                                AE.Episodes[n] = episode;
                            }
                        }
                        else
                            AE.Episodes[n] = E;
                    }
                }
                await _cs.CreateOrUpdateAsync(AE);
            }
            i++;
        }
        await message.ModifyAsync(x => x.Content = $"Done 100% / 100%");
    }
    #endregion
}
