using Discord;
using Scraper_Bot.Interfaces.Services;
using System.Diagnostics;

namespace Scraper_Bot;
public class SlashBuilder
{
    private readonly DiscordSocketClient _client;
    private ulong _guildId;
    private SocketGuild _guild;

    private readonly ICrunchyrollService _cs;

    public SlashBuilder(IServiceProvider service, IConfiguration conf)
    {
        _client = service.GetRequiredService<DiscordSocketClient>();
        _guildId = ulong.Parse(conf["GuildID"]);
        _cs = service.GetRequiredService<ICrunchyrollService>();
    }

    public async Task<SlashBuilder> Start()
    {
        _guild = _client.GetGuild(_guildId);
        await _guild.DeleteApplicationCommandsAsync();

        return this;
    }

    public async Task<SlashBuilder> Tools()
    {
        var tools = new SlashCommandBuilder();

        tools.WithName("tools")
             .WithDescription("admin tools")
             .AddOption(new SlashCommandOptionBuilder().WithName("purge").WithDescription("use a number to delete a mount of messages or all for all messages").WithType(ApplicationCommandOptionType.String));
        await SlashCommandCreator(tools);

        return this;
    }

    public async Task<SlashBuilder> LFG()
    {
        var lfg = new SlashCommandBuilder();

        lfg.WithName("lfg").WithDescription("add an lfg request into thread")
            .AddOption(new SlashCommandOptionBuilder().WithName("url").WithDescription("enter a valid steam url into").WithType(ApplicationCommandOptionType.String).WithRequired(true))
            .AddOption(new SlashCommandOptionBuilder().WithName("description").WithDescription("insert a little text for what u search").WithType(ApplicationCommandOptionType.String).WithRequired(false));

        await SlashCommandCreator(lfg);

        return this;
    }

    public async Task<SlashBuilder> Scraper()
    {
        try
        {
            var scraper = new SlashCommandBuilder();

            scraper
            .WithName("scraper")
            .WithDescription("scrape data")
            // Crunchyroll
            .AddOption(new SlashCommandOptionBuilder().WithName("crunchyroll").WithDescription("crunchyroll scraper").WithType(ApplicationCommandOptionType.SubCommand)
                .AddOption(new SlashCommandOptionBuilder().WithName("url").WithDescription("scrape anime per url").WithType(ApplicationCommandOptionType.String))
                .AddOption(new SlashCommandOptionBuilder().WithName("update").WithDescription("update crunchyroll").WithType(ApplicationCommandOptionType.Integer)
                      .AddChoice("fullupdate", 1)
                      .AddChoice("weeklyupdate", 2)
                      .AddChoice("dailyupdate", 3)))
            //Steam
             .AddOption(new SlashCommandOptionBuilder().WithName("steam").WithDescription("steam scraper").WithType(ApplicationCommandOptionType.SubCommand)
                .AddOption(new SlashCommandOptionBuilder().WithName("game").WithDescription("need a steam game url").WithType(ApplicationCommandOptionType.String))
                .AddOption(new SlashCommandOptionBuilder().WithName("wishlist").WithDescription("nedd a url from a steam wishlist").WithType(ApplicationCommandOptionType.String))
                .AddOption(new SlashCommandOptionBuilder().WithName("category").WithDescription("choose between 1-4").WithType(ApplicationCommandOptionType.Integer)
                       .AddChoice("neu & angesagt", 1)
                       .AddChoice("topseller", 2)
                       .AddChoice("beliebt und bald verfügbar", 3)
                       .AddChoice("angebote", 4))
                .AddOption(new SlashCommandOptionBuilder().WithName("update").WithDescription("soft or hard update of all steam games").WithType(ApplicationCommandOptionType.String)
                        .AddChoice("fullupdate", "hard")
                        .AddChoice("softupdate", "soft"))
                .AddOption(new SlashCommandOptionBuilder().WithName("user").WithDescription("search for an user profile").WithType(ApplicationCommandOptionType.String)));


            await SlashCommandCreator(scraper);
        }
        catch (Exception err)
        {

            Console.WriteLine(err.Message);
        }
        return this;
    }

    private async Task SlashCommandCreator(SlashCommandBuilder slashCommandBuilder)
    {
        await _guild.CreateApplicationCommandAsync(slashCommandBuilder.Build());
    }
}
