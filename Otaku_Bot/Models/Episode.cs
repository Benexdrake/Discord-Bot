﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otaku_Bot.Models
{
    public class Episode
    {
        public string Id { get; set; }
        public int EpisodeNr { get; set; }
        public string SeasonName { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }
        public string AnimeId { get; set; }
    }
}
