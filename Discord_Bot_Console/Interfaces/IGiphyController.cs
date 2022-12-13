using GiphyDotNet.Model.Results;

namespace Discord_Bot_Console.Interfaces
{
    public interface IGiphyController
    {
        Task<GiphyRandomResult> SearchGifs(string gif);
    }
}