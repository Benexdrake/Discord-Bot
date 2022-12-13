namespace Discord_Bot_Console.Models;

public class DiscordEmbedBuilder
{
    public async Task<EmbedBuilder> PokemonEmbed(Pokemon p)
    {
        EmbedBuilder embed = new EmbedBuilder();
        var embedFieldList = new List<EmbedFieldBuilder>();
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "KP",
            Value = p.KP
        });

        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Attack",
            Value = p.Attack
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Defensiv",
            Value = p.Defensiv
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "SP Attack",
            Value = p.SPAttack
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "SP Defensiv",
            Value = p.SPDefensiv
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Initiative",
            Value = p.Initiative
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Size",
            Value = p.Size
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Sex",
            Value = p.Sex
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Weight",
            Value = p.Weight
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = false,
            Name = "Category",
            Value = p.Category
        });


        var skillnames = p.SkillName.Split(';');
        var skilldescrs = p.SkillDescription.Split(";");
        for (int i = 0; i < skillnames.Length; i++)
        {
            embedFieldList.Add(new EmbedFieldBuilder()
            {
                IsInline = false,
                Name = skillnames[i],
                Value = skilldescrs[i]
            });
        }

        var footer = new EmbedFooterBuilder()
        {
            Text = $"Type: {p.Type}\nWeakness: {p.Weakness}"
        };

        embed.ImageUrl = p.ImageUrl;
        embed.Author = new EmbedAuthorBuilder() { IconUrl = "https://cdn-icons-png.flaticon.com/512/528/528101.png", Name = "Pokemon", Url = "https://www.pokemon.com/de/pokedex/" };
        embed.Description = p.Description;
        embed.Title = $"NR: {p.Nr} - {p.Name}";
        embed.Url = p.Url;
        embed.Color = Color.Blue;
        embed.Fields = embedFieldList;
        embed.Footer = footer;
        return embed;
    }

    public async Task<EmbedBuilder> IMDBEmbed(Movie m)
    {
        var embed = new EmbedBuilder();
        var embedFieldList = new List<EmbedFieldBuilder>();

        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Genres",
            Value = m.Genres
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Runtime",
            Value = m.Runtime
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Main Cast",
            Value = m.MainCast
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Budget",
            Value = m.Budget
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Director",
            Value = m.Director
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Known as",
            Value = m.KnownAs
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = false,
            Name = "Production Companies",
            Value = m.ProductionCompanies
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = false,
            Name = "Script",
            Value = m.Script
        });

        embed.Author = new EmbedAuthorBuilder() { IconUrl = "https://cdn.icon-icons.com/icons2/70/PNG/512/imdb_14058.png", Name = "IMDB", Url = "https://www.imdb.com/" };
        embed.Description = m.Description;
        embed.Title = m.Title;
        embed.Url = m.Url;
        embed.ImageUrl = m.ImgUrl;
        embed.Fields = embedFieldList;
        embed.Footer = new EmbedFooterBuilder()
        {
            Text = $"Rating: {m.Rating}\nRelease: {m.ReleaseDate}"
        };

        return embed;
    }

    public async Task<EmbedBuilder> AnimeEmbed(Anime a)
    {
        var embed = new EmbedBuilder();
        var embedFieldList = new List<EmbedFieldBuilder>();

        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Rating",
            Value = a.Rating
        });
        embedFieldList.Add(new EmbedFieldBuilder()
        {
            IsInline = true,
            Name = "Episodes",
            Value = a.Episodes
        });

        embed.Description = a.Description;
        embed.Title = a.Name;
        embed.Url = a.Url;
        embed.Author = new EmbedAuthorBuilder() { IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/08/Crunchyroll_Logo.png/800px-Crunchyroll_Logo.png", Name = "Crunchyroll", Url = "https://www.crunchyroll.com" };
        embed.Color = Color.Orange;
        embed.ImageUrl = a.ImageUrl;
        embed.Fields = embedFieldList;
        embed.Footer = new EmbedFooterBuilder()
        {
            Text = $"Tags: {a.Tags}\nPublisher: {a.Publisher}"
        };
        return embed;
    }

    public async Task<EmbedBuilder> GifEmbed(GiphyRandomResult gif)
    {
        var embedfieldList = new List<EmbedFieldBuilder>();
        var author = new EmbedAuthorBuilder();
        var footer = new EmbedFooterBuilder();
        var embed = new EmbedBuilder();
        if (gif != null)
        {
            if (gif.Data.Caption is not null)
            {
                embedfieldList.Add(new EmbedFieldBuilder()
                {
                    IsInline = true,
                    Name = "Caption",
                    Value = gif.Data.Caption
                });
            }

            if (gif.Data.Rating is not null)
            {
                embedfieldList.Add(new EmbedFieldBuilder()
                {
                    IsInline = true,
                    Name = "Rating",
                    Value = gif.Data.Rating
                });
            }

            if (gif.Data.Source is not null)
            {
                embedfieldList.Add(new EmbedFieldBuilder()
                {
                    IsInline = false,
                    Name = "Source",
                    Value = gif.Data.Source
                });
            }
            string thumbnail = string.Empty;
            if (gif.Data.User is not null)
            {
                if (gif.Data.User.AvatarUrl is not null)
                {
                    thumbnail = gif.Data.User.AvatarUrl;
                    embed.ThumbnailUrl = thumbnail;
                }
                var f = new EmbedFooterBuilder()
                {
                    IconUrl = gif.Data.ContentUrl,
                    Text = gif.Data.Username
                };
                var a = new EmbedAuthorBuilder()
                {
                    IconUrl = thumbnail,
                    Name = gif.Data.User.DisplayName,
                    Url = gif.Data.User.ProfileUrl
                };
                author = a;
                footer = f;
            }

            embed.Title = gif.Data.Title;
            embed.Description = gif.Data.Username;
            embed.ImageUrl = gif.Data.Images.Original.Url;
            embed.Color = Color.Blue;
            embed.Fields = embedfieldList;
            embed.Footer = footer;
            embed.Author = author;
            embed.Timestamp = DateTime.Parse(gif.Data.ImportDatetime);
            embed.Url = gif.Data.Url;
        }
        else
        {
            Log.Logger.Error("Something went wrong!");
        }
        return embed;
    }

    public async Task<EmbedBuilder> HelpEmbed(Help h)
    {
        var embed = new EmbedBuilder();

        var embedfield = new List<EmbedFieldBuilder>();

        string commands = string.Empty;

        for (int i = 0; i < h.Parameters.Length; i++)
        {
            if (i < h.Parameters.Length - 1)
            {
                commands += h.Parameters[i] + Environment.NewLine;
            }
            else
                commands += h.Parameters[i];
        }

        embedfield.Add(new EmbedFieldBuilder()
        {
            IsInline = false,
            Name = "Commands",
            Value = commands
        });

        embedfield.Add(new EmbedFieldBuilder()
        {
            IsInline = false,
            Name = "Information",
            Value = h.Description
        });

        embed.Title = h.CommandName;
        embed.Fields = embedfield;


        return embed;
    }

    public async Task<EmbedBuilder> HelpListEmbed(List<Help> h)
    {
        var embed = new EmbedBuilder();
        var embedfield = new List<EmbedFieldBuilder>();

        foreach (var help in h)
        {
            string commands = string.Empty;

            for (int i = 0; i < help.Parameters.Length; i++)
            {
                if (i < help.Parameters.Length - 1)
                {
                    commands += help.Parameters[i] + Environment.NewLine;
                }
                else
                    commands += help.Parameters[i];
            }

            embedfield.Add(new EmbedFieldBuilder()
            {
                IsInline = false,
                Name = help.CommandName,
                Value = commands
            });
        }
        embed.Fields = embedfield;
        embed.Title = "Help";

        return embed;
    }
}
