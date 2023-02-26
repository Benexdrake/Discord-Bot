using Discord;

namespace Discord_Bot.Embeds;
public class CrunchyrollEmbed
{
    public async Task<EmbedBuilder> AnimeEmbed(Anime a)
    {
        var embed = new EmbedBuilder();

        var embedFieldList = new List<EmbedFieldBuilder>();

        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Rating",
            Value = a.Rating.ToString().Replace(",", ".")
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Episodes",
            Value = a.Episodes.ToString()
        });

        var author = new EmbedAuthorBuilder() { IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/08/Crunchyroll_Logo.png/800px-Crunchyroll_Logo.png", Name = "Crunchyroll", Url = "https://www.crunchyroll.com" };
        var footer = new EmbedFooterBuilder()
        { Text = $"Genres: {a.Tags}\nPublisher: {a.Publisher}" };
        embed.WithDescription(a.Description)
             .WithTitle(a.Name)
             .WithUrl(a.Url)
             .WithAuthor(author)
             .WithColor(Color.Orange)
             .WithImageUrl(a.ImageUrl)
             .WithFields(embedFieldList)
             .WithFooter(footer);
        return embed;
    }
}
