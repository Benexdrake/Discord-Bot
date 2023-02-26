using Discord;

namespace Discord_Bot;
public class SlashBuilder
{
    private readonly DiscordSocketClient _client;
    private ulong _guildId;
    private SocketGuild _guild;

    public SlashBuilder(IServiceProvider service, IConfiguration conf)
    {
        _client = service.GetRequiredService<DiscordSocketClient>();
        _guildId = ulong.Parse(conf["GuildID"]);
    }

    public async Task<SlashBuilder> Start()
    {
        _guild = _client.GetGuild(_guildId);
        return this;
    }

    public async Task<SlashBuilder> Scraper()
    {
        try
        {
            await _guild.DeleteApplicationCommandsAsync();
            var guildCommand = new SlashCommandBuilder();

            guildCommand
            .WithName("scraper")
            .WithDescription("scrape data")
            .AddOption(new SlashCommandOptionBuilder().WithName("crunchyroll").WithDescription("crunchyroll scraper")
            .WithType(ApplicationCommandOptionType.SubCommand)
                .AddOption(new SlashCommandOptionBuilder().WithName("update").WithDescription("update crunchyroll").WithType(ApplicationCommandOptionType.Integer)
                      .AddChoice("fullupdate", 1)
                      .AddChoice("weeklyupdate", 2)
                      .AddChoice("dailyupdate", 3))
                .AddOption(new SlashCommandOptionBuilder().WithName("url").WithDescription("scrape anime per url").WithType(ApplicationCommandOptionType.String)));


            await SlashCommandCreator(guildCommand);
        }
        catch (Exception err)
        {

            Console.WriteLine(err.Message);
        }
        return this;
    }

    public async Task<SlashBuilder> Crunchyroll()
    {
        var crunchyroll = new SlashCommandBuilder()
                        .WithName("crunchyroll")
                        .WithDescription("searching for an anime on crunchyroll")
                        .AddOption(new SlashCommandOptionBuilder().WithName("name").WithDescription("search with name").WithType(ApplicationCommandOptionType.String))
                        .AddOption(new SlashCommandOptionBuilder().WithName("url").WithDescription("search with url").WithType(ApplicationCommandOptionType.String))
                        .AddOption(new SlashCommandOptionBuilder().WithName("rating").WithDescription("search with rating 0-5").WithType(ApplicationCommandOptionType.Number))
                        .AddOption(new SlashCommandOptionBuilder().WithName("episodes").WithDescription("search with episodes count").WithType(ApplicationCommandOptionType.Integer))
                        .AddOption(new SlashCommandOptionBuilder().WithName("publisher").WithDescription("search with publisher name").WithType(ApplicationCommandOptionType.String))
                        .AddOption(new SlashCommandOptionBuilder().WithName("genre").WithDescription("choose what genre you want to see")
                          .AddChoice("action", "action")
                          .AddChoice("adventure", "adventure")
                          .AddChoice("comedy", "comedy")
                          .AddChoice("drama", "comedy")
                          .AddChoice("fantasy", "fantasy")
                          .AddChoice("music", "music")
                          .AddChoice("romance", "romance")
                          .AddChoice("science-fiction", "science-fiction")
                          .AddChoice("seinen", "seinen")
                          .AddChoice("shojo", "shojo")
                          .AddChoice("shonen", "shonen")
                          .AddChoice("slice-of-life", "slice-of-life")
                          .AddChoice("sport", "sport")
                          .AddChoice("supernatural", "supernatural")
                          .AddChoice("thriller", "thriller")
                          .WithType(ApplicationCommandOptionType.String))
                        .WithDefaultPermission(true);

        await SlashCommandCreator(crunchyroll);

        return this;
    }

    public async Task<SlashBuilder> Twitch()
    {
        var twitch = new SlashCommandBuilder()
                     .WithName("twitch")
                     .WithDescription("search for a twitch profil")
                     .AddOption(new SlashCommandOptionBuilder().WithName("url").WithDescription("enter the twitch profil url").WithType(ApplicationCommandOptionType.String))
                     .WithDefaultPermission(true);

        await SlashCommandCreator(twitch);

        return this;
    }


    private async Task SlashCommandCreator(SlashCommandBuilder slashCommandBuilder)
    {
        await _guild.CreateApplicationCommandAsync(slashCommandBuilder.Build());
    }
}
