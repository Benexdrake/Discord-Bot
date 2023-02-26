using Discord;
using Webscraper_API.Scraper.Twitch.Models;

namespace Discord_Bot.Embeds;

public class TwitchEmbed
{

    public async Task<EmbedBuilder> Embed(User u)
    {
        var embed = new EmbedBuilder();
        var embedFieldList = new List<EmbedFieldBuilder>();

        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Follower",
            Value = u.Follower
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = false,
            Name = "Information",
            Value = u.Information
        });
        var author = new EmbedAuthorBuilder() 
        { IconUrl = u.AvatarUrl, 
          Name = u.Name, 
          Url = u.ProfilUrl };

        var games = string.Empty;
        if(u.LastStreamedGames is not null)
        {
            for (int i = 0; i < u.LastStreamedGames.Length; i++)
            {
                if (i < u.LastStreamedGames.Length - 1)
                    games += u.LastStreamedGames[i].Name + ",\n";
                else
                    games += u.LastStreamedGames[i].Name;
            }
        }

        var footer = new EmbedFooterBuilder()
        {Text = $"Last Played Games:\n{games}"};

        embed.WithDescription(u.Description)
             .WithThumbnailUrl(u.AvatarUrl)
             .WithTitle(u.Name)
             .WithUrl(u.ProfilUrl)
             .WithAuthor(author)
             .WithColor(Color.Purple)
             .WithImageUrl(u.BannerUrl)
             .WithFields(embedFieldList)
             .WithFooter(footer);

        return embed;
    }
}
