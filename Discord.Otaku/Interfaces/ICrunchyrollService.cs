using Otaku_Bot.Models;

namespace Otaku_Bot.Interfaces
{
    public interface ICrunchyrollService
    {
        Task CreateOrUpdateAsync(Anime_Episodes AE);
        Task<Anime_Episodes[]> GetAllAnimeEpisodes();
        Task<Anime> GetAnimeByIdAsync(string id);
        Task<Anime[]> GetAnimeByPublisherAsync(string publisher);
        Task<Anime[]> GetAnimesAsync();
        Task<Anime[]> GetAnimesByEpisodesAsync(int episodes);
        Task<Anime[]> GetAnimesByGenreAsync(string genre);
        Task<Anime[]> GetAnimesByNameAsync(string param);
        Task<Anime[]> GetAnimesByRatingAsync(double rating);
        Task<Episode[]> GetEpisodesByAnimeIdAsync(string animeId);
        Task<Anime> GetRandomAnimeAsync();
    }
}