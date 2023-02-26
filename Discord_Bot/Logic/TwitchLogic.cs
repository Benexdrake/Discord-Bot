using Discord;
using Discord_Bot.Embeds;

namespace Discord_Bot.Logic
{
    public class TwitchLogic
    {
        private readonly ITwitch_API _api;
        private readonly TwitchEmbed _embed;
        public TwitchLogic(IServiceProvider service)
        {
            _api = service.GetRequiredService<ITwitch_API>();
            _embed = service.GetRequiredService<TwitchEmbed>();
        }

        public async Task GetTwitchProfil(IUserMessage message, string url)
        {
            await message.ModifyAsync(x => x.Content = $"Looking for {url}");

            var user = _api.GetTwitchProfil(url).Result;

            var embed = _embed.Embed(user).Result;

            await message.ModifyAsync(x => x.Content = $"Found {user.Name}");
            await message.ModifyAsync(x => x.Embed = embed.Build());
        }
    }
}
