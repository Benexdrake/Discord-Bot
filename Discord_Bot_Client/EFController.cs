using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_Client
{
    public class EFController
    {
        private readonly ServiceProvider _service;
        private readonly CrunchyrollDBContext _crunchyrollDBContext;

        public EFController(ServiceProvider service)
        {
            _service = service;
            _crunchyrollDBContext = service.GetRequiredService<CrunchyrollDBContext>();
        }

        // Crunchyroll
        private void SaveAnime(Anime anime)
        {
            _crunchyrollDBContext.Animes.Add(anime);
            _crunchyrollDBContext.SaveChanges();
        }


        private void SaveEpisodes(Episode[] episodes)
        {
            foreach (var episode in episodes)
            {
                _crunchyrollDBContext.Episodes.Add(episode);
            }
            _crunchyrollDBContext.SaveChanges();
        }
    }
}
