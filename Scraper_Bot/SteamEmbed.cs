using Discord;

namespace Scraper_Bot;

public class SteamEmbed
{
    public async Task<EmbedBuilder> GameEmbed(SteamGame g)
    {
        var embed = new EmbedBuilder();

        var embedFieldList = new List<EmbedFieldBuilder>();

        if (!string.IsNullOrWhiteSpace(g.ReleaseDate))
            embedFieldList.Add(new EmbedFieldBuilder()
            {
                IsInline = false,
                Name = "Release Date:",
                Value = g.ReleaseDate
            });
        if (!string.IsNullOrWhiteSpace(g.Publisher))
            embedFieldList.Add(new EmbedFieldBuilder()
            {
                IsInline = false,
                Name = "Publisher:",
                Value = g.Publisher
            });
        if (!string.IsNullOrWhiteSpace(g.DevTeam))
            embedFieldList.Add(new EmbedFieldBuilder()
            {
                IsInline = false,
                Name = "Developer:",
                Value = g.DevTeam
            });


        for (int i = 0; i < g.Prices.Length; i++)
        {
            if (g.Prices[i].IsFree)
            {
                embedFieldList.Add(new EmbedFieldBuilder()
                {
                    IsInline = false,
                    Name = g.Prices[i].Title,
                    Value = "Free"
                });
            }
            else if (g.Prices[i].DiscountPrice > 0)
            {
                embedFieldList.Add(new EmbedFieldBuilder()
                {
                    IsInline = false,
                    Name = g.Prices[i].Title,
                    Value = g.Prices[i].DiscountPrice + "€"
                });
            }
            else
            {
                embedFieldList.Add(new EmbedFieldBuilder()
                {
                    IsInline = false,
                    Name = g.Prices[i].Title,
                    Value = g.Prices[i].OriginalPrice + "€"
                });
            }
        }

        var author = new EmbedAuthorBuilder() { IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/512px-Steam_icon_logo.svg.png", Name = "Steam", Url = "https://store.steampowered.com" };
        string tags = string.Empty;

        for (int i = 0; i < g.Tags.Length; i++)
        {
            if (i < g.Tags.Length - 1)
                tags += g.Tags[i] + ", ";
            else
                tags += g.Tags[i];
        }

        var footer = new EmbedFooterBuilder()
        { Text = $"Tags: {tags}" };

        embed.WithDescription(g.DescriptionSnippet)
             .WithTitle(g.Title)
             .WithUrl(g.Url)
             .WithAuthor(author)
             .WithColor(Color.Blue)
             .WithImageUrl(g.GameImageUrl)
             .WithFields(embedFieldList)
             .WithFooter(footer);
        return embed;
    }

    public async Task<EmbedBuilder> UserEmbed(SteamUser user)
    {
        var embed = new EmbedBuilder();

        var embedFieldList = new List<EmbedFieldBuilder>();

        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Buyed Games:",
            Value = user.Games.Length
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Reviws:",
            Value = user.Reviews
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Wishlist:",
            Value = user.GamesOnWishlist
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = false,
            Name = "Trophys:",
            Value = user.Trophys
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = false,
            Name = "Platin Trophys:",
            Value = user.Platin
        });

        var author = new EmbedAuthorBuilder()
        {
            IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/512px-Steam_icon_logo.svg.png",
            Name = "Steam",
            Url = "https://store.steampowered.com"
        };

        var footer = new EmbedFooterBuilder()
        { Text = $"Level {user.Level}" };

        embed.WithDescription(user.ProfilText)
             .WithThumbnailUrl(user.BadgeIconUrl)
             .WithTitle(user.Username)
             .WithUrl(user.ProfilUrl)
             .WithAuthor(author)
             .WithColor(Color.DarkPurple)
             .WithImageUrl(user.AvatarUrl)
             .WithFields(embedFieldList)
             .WithFooter(footer);

        return embed;
    }
}
