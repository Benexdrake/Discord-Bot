using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Logic;
using Webscraper_API;
using Webscraper_API.Scraper.Crunchyroll.Controllers;
using Webscraper_API.Scraper.Insight_Digital_Handy.Controllers;
using Webscraper_API.Scraper.Pokemons.Controller;
using Webscraper_API.Scraper.Steam.Controllers;

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
        //service.AddSingleton<IDiscordBotModule, DiscordBotModule>();
        //service.AddSingleton<DiscordEmbedBuilder>();
        //service.AddScoped<IGiphyController, GiphyController>();

        service.AddScoped<CrunchyrollService>();
        service.AddScoped<SteamService>();
        service.AddScoped<InsightDigitalService>();
        service.AddScoped<PokemonService>();

        service.AddScoped<CrunchyrollLogic>();
        service.AddScoped<SteamLogic>();
        service.AddScoped<InsightDigitalLogic>();
        service.AddScoped<PokemonLogic>();

        service.AddScoped<ISteam_Api,Steam_Api>();
        service.AddScoped<ICR_API, CR_API>();
        service.AddScoped<IID_API, ID_API>();
        service.AddScoped<IPokemon_API, Pokemon_API>();

        service.AddSingleton<HttpClient>();
        service.AddSingleton<Browser>();

        service.AddScoped<PdfSeperator>();
        service.AddScoped<PdfRandomizer>();

        service.AddSingleton<DiscordSocketClient>();

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