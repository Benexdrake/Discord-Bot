using Discord;
using Scraper_Bot.Commands.Slash;
using Webscraper_API.Scraper.Amazon.Controllers;
using Webscraper_API.Scraper.TVProgramm.Controllers;
using Webscraper_API.Scraper.Twitch.Controllers;

IConfiguration conf;

var builder = new ConfigurationBuilder();
BuildConfig(builder);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

conf = builder.Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, service) =>
    {
        service.AddScoped<ICrunchyrollService,CrunchyrollService>();
        service.AddScoped<ISteamService, SteamService>();
        service.AddScoped<IInsightDigitalService, InsightDigitalService>();
        service.AddScoped<IPokemonService, PokemonService>();

        service.AddScoped<CrunchyrollLogic>();
        service.AddScoped<SteamLogic>();
        service.AddScoped<InsightDigitalLogic>();
        service.AddScoped<PokemonLogic>();

        service.AddScoped<ISteam_Api,Steam_Api>();
        service.AddScoped<ICR_API, CR_API>();
        service.AddScoped<IID_API, ID_API>();
        service.AddScoped<IPokemon_API, Pokemon_API>();
        service.AddScoped<ITwitch_API, Twitch_API>();
        service.AddScoped<TVDirekt_API>();
        service.AddScoped<Amazon_API>();

        service.AddSingleton<SlashBuilder>();
        service.AddSingleton<ScraperSlashCommands>();
        service.AddSingleton<ToolsSlashCommands>();

        service.AddSingleton<SteamEmbed>();

        service.AddSingleton<HttpClient>();
        service.AddSingleton<Browser>();

        service.AddSingleton(GetDiscordClient());
        service.AddSingleton<CommandService>();
        service.AddHostedService<Bot>();
    })
    .UseSerilog()
    .Build();

host.Start();
static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
}

static DiscordSocketClient GetDiscordClient()
{
    var socketConfig = new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.GuildBans
    };

    return new DiscordSocketClient(socketConfig);
}