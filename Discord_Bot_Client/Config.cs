using Discord_Bot_Client.Commands.Modules;
using Discord_Bot_Client.SlashCommands;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

namespace Discord_Bot_Client
{
    public static class Config
    {
        public static async Task<IHostBuilder> CreateHost(IConfiguration conf)
        {
            return Host.CreateDefaultBuilder()
            .ConfigureServices((context, service) =>
        {
            service.AddSingleton<Browser>();
            service.AddScoped<ICR_API, CR_API>();
            service.AddScoped<IIMDb_API, IMDb_API>();
            service.AddScoped<IPokemon_API, Pokemon_API>();
            service.AddScoped<ITCG_API, TCG_API>();
            service.AddScoped<IHonda_Api, Honda_Api>();
            service.AddScoped<ITCG_M_API, TCG_M_API>();

            service.AddSingleton<DiscordSocketClient>();
            service.AddSingleton<CommandService>();
            service.AddSingleton<InteractionService>();

            service.AddSingleton<SlashCommandsBuilder>();
            service.AddSingleton<DiscordBotModule>();
            service.AddSingleton<DiscordBotSlashModule>();

            service.AddSingleton<ScrapingModule>();

            service.AddDbContext<CrunchyrollDBContext>(options => options.UseSqlServer(conf.GetConnectionString("Crunchyroll")));
            service.AddDbContext<ImdbDBContext>(options => options.UseSqlServer(conf.GetConnectionString("IMDB")));
            service.AddDbContext<PokemonDBContext>(options => options.UseSqlServer(conf.GetConnectionString("Pokemon")));
            service.AddDbContext<PokemonCardDbContext>(options => options.UseSqlServer(conf.GetConnectionString("PokemonCards")));
            service.AddDbContext<HondaPartsDbContext>(options => options.UseSqlServer(conf.GetConnectionString("HondaParts")));
            service.AddHostedService<Bot>();
        });
        }

        public static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        public static Logger GetLogger(ConfigurationBuilder builder)
        {
            return new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        }
    }
}
