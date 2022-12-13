using Discord_Bot_Console.Interfaces;

namespace Discord_Bot_Console.Controllers
{
    public class GiphyController : IGiphyController
    {
        private readonly IKeys _keys;

        public GiphyController(IServiceProvider service)
        {
            _keys = (IKeys)service.GetService(typeof(IKeys));
        }

        public async Task<GiphyRandomResult> SearchGifs(string gif)
        {
            Giphy g = new Giphy(_keys.Giphy);
            RandomParameter rp = new RandomParameter();
            rp.Tag = gif;
            return g.RandomGif(rp).Result;
        }
    }
}
