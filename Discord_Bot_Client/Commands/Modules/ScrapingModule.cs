using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_Client.Commands.Modules
{
    public class ScrapingModule : ModuleBase<SocketCommandContext>
    {
        private readonly IServiceProvider _service;
        private readonly CrunchyrollDBContext _crunchyrollDBContext;
        private readonly ICR_API _cr_Api;
        private readonly Browser _browser;

        public ScrapingModule(IServiceProvider service)
        {
            _service = service;
            _crunchyrollDBContext = service.GetRequiredService<CrunchyrollDBContext>();
            _cr_Api = service.GetRequiredService<ICR_API>();

            _browser= service.GetRequiredService<Browser>();
            //_browser.WebDriver = _browser.FirefoxDebug();
        }

        [RequireRole("Scraper")]
        [Command("crunchyroll", true)]
        public async Task CrunchyrollScraping([Remainder] string param)
        {
            bool update = false;
            var urls = new List<string>();

            var message = Context.Message.ReplyAsync("Please wait").Result;
            if (param.Contains("full"))
            {
                update = true;

                var u = _cr_Api.GetAllAnimeUrlsAsync();
                await Task.Delay(1000);
                while (!u.IsCompleted)
                {
                    var m = _cr_Api.Message;
                    await message.ModifyAsync(x => x.Content = m);
                    await Task.Delay(1000);
                }
                urls = u.Result.ToList();
            }
            else if (param.Contains("daily"))
            {
                update = true;
                // Daily Update
                urls = _cr_Api.GetDailyUpdateAsync().Result.ToList();
            }

            if (update)
            {
                await message.ModifyAsync(x => x.Content = $"Found {urls.Count} Animes");
                await Task.Delay(1000);
                for (int i = 0; i < urls.Count; i++)
                {
                    await message.ModifyAsync(x => x.Content = $"{i+1}/{urls.Count} - {Helper.Percent(i, urls.Count)}/100% - {urls[i]}");
                    var a = _crunchyrollDBContext.Animes.Where(x => x.Url.Equals(urls[i])).FirstOrDefault();
                    if (a is null)
                    {
                        _cr_Api.Episodes = 0;
                        var AE = _cr_Api.GetAnimewithEpisodes(urls[i], 4000).Result;
                        if (AE is not null)
                        {
                            _crunchyrollDBContext.Animes.Add(AE.Anime);

                            foreach (var episode in AE.Episodes)
                            {
                                var e = _crunchyrollDBContext.Episodes.Where(a => a.Id.Equals(episode.Id)).FirstOrDefault();
                                if (e is null)
                                {
                                    _crunchyrollDBContext.Episodes.Add(episode);
                                }
                            }
                            _crunchyrollDBContext.SaveChanges();
                        }
                    }
                    else
                    {   
                        var AE = _cr_Api.GetAnimewithEpisodes(urls[i], 3000).Result;
                        if(AE is not null)
                        {
                            if (a.Episodes != AE.Anime.Episodes || a.Rating == 0)
                            {
                                a.Episodes = AE.Anime.Episodes;
                                a.Rating= AE.Anime.Rating;
                            }

                            foreach (var episode in AE.Episodes)
                            {
                                var e = _crunchyrollDBContext.Episodes.Where(a => a.Id.Equals(episode.Id)).FirstOrDefault();
                                if (e is null)
                                {
                                    _crunchyrollDBContext.Episodes.Add(episode);
                                }
                            }
                            _crunchyrollDBContext.SaveChanges();
                        }
                    }
                }
            }

            if(param.Contains("details"))
            {
                var episodes = _crunchyrollDBContext.Episodes.Where(x => x.Description == "" || x.ReleaseDate == "").ToList();

                for (int i = 0; i < episodes.Count; i++)
                {
                    await message.ModifyAsync(x => x.Content = $"{i + 1}/{episodes.Count} - {Helper.Percent(i, episodes.Count)}/100% - {episodes[i].Title}");
                    var e = _cr_Api.GetEpisodeDetails(episodes[i]).Result;
                    
                    var EpisodeDB = _crunchyrollDBContext.Episodes.Where(x => x.Id.Equals(e.Id)).FirstOrDefault();
                    EpisodeDB.ReleaseDate = e.ReleaseDate;
                    EpisodeDB.Description= e.Description;

                    _crunchyrollDBContext.SaveChanges();
                }
            }

            await message.ModifyAsync(x => x.Content = $"Done...");
            if (param.Contains("json"))
            {
                var animes = JsonConvert.SerializeObject(_crunchyrollDBContext.Animes.ToList(), Formatting.Indented);
                var episodes = JsonConvert.SerializeObject(_crunchyrollDBContext.Episodes.ToList(), Formatting.Indented);

                string dir = "Crunchyroll";

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                await File.WriteAllTextAsync(dir + "\\animes.json", animes);
                await File.WriteAllTextAsync(dir + "\\episodes.json", episodes);

                string zipfilePath = dir + "\\" + dir + ".zip";
                string zipfile = dir + ".zip";

                if (File.Exists(zipfilePath))
                    File.Delete(zipfilePath);


                using var archive = ZipFile.Open(zipfilePath, ZipArchiveMode.Create);

                var entry = archive.CreateEntryFromFile(dir + "\\animes.json", "animes.json", CompressionLevel.Optimal);
                var entry2 = archive.CreateEntryFromFile(dir + "\\episodes.json", "episodes.json", CompressionLevel.Optimal);

                archive.Dispose();

                if (File.Exists(zipfilePath))
                {
                    var attachments = new List<FileAttachment>();
                    FileAttachment f = new FileAttachment(zipfilePath);
                    attachments.Add(f);
                    message.ModifyAsync(x => x.Content = "Here is your File");
                    message.ModifyAsync(x => x.Attachments = attachments);
                }
                

            }
        }
    }
}
