namespace Discord_Bot.Interfaces.Services
{
    public interface ICrunchyrollService
    {
        Task CreateOrUpdateAsync(Anime_Episodes AE);
        Task<Anime_Episodes[]> GetAllAnimeEpisodes();
        Task<Anime> GetAnimeByIdAsync(string id);
        Task<Anime[]> GetAnimesAsync();
        Task<Anime[]> GetAnimesByEpisodesAsync(int episodes);
        Task<Anime[]> GetAnimesByGenreAsync(string genre);
        Task<Anime[]> GetAnimesByNameAsync(string param);
        Task<Anime[]> GetAnimesByRatingAsync(double rating);
        Task<Anime[]> GetAnimeByPublisherAsync(string publisher);
        Task<Episode[]> GetEpisodesByAnimeIdAsync(string animeId);
        Task<Anime> GetRandomAnimeAsync();
    }
}