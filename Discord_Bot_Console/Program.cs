

using Discord_Bot_Console.Modules;

IConfiguration conf;

var builder = new ConfigurationBuilder();
BuildConfig(builder);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Information("Application Starting");

conf = builder.Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, service) =>
    {
        service.AddScoped<IKeys,Keys>();
        service.AddSingleton<IDiscordBotModule, DiscordBotModule>();
        service.AddSingleton<DiscordEmbedBuilder>();
        service.AddScoped<IGiphyController,GiphyController>();
        service.AddScoped<IKeys, Keys>();
        service.AddSingleton<DiscordEmbedBuilder>();
        service.AddScoped<IGiphyController, GiphyController>();
        service.AddSingleton<Browser>();
        service.AddScoped<ICR_API, CR_API>();
        service.AddScoped<IIMDb_API, IMDb_API>();
        service.AddScoped<IPokemon_API, Pokemon_API>();
        service.AddScoped<ITCG_API, TCG_API>();
        service.AddScoped<IHonda_Api, Honda_Api>();
        service.AddScoped<ITCG_M_API, TCG_M_API>();

        service.AddScoped<PdfSeperator>();
        service.AddScoped<PdfRandomizer>();

        service.AddSingleton<DiscordSocketClient>();
        //service.AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig()
        //{
        //    GatewayIntents = GatewayIntents.AllUnprivileged,
        //    AlwaysDownloadUsers= true
        //}));
        service.AddSingleton<CommandService>();

        service.AddDbContext<CrunchyrollDBContext>(options => options.UseSqlServer(conf.GetConnectionString("Crunchyroll")));
        service.AddDbContext<ImdbDBContext>(options => options.UseSqlServer(conf.GetConnectionString("IMDB")));
        service.AddDbContext<PokemonDBContext>(options => options.UseSqlServer(conf.GetConnectionString("Pokemon")));
        service.AddDbContext<PokemonCardDbContext>(options => options.UseSqlServer(conf.GetConnectionString("PokemonCards")));
        service.AddDbContext<HondaPartsDbContext>(options => options.UseSqlServer(conf.GetConnectionString("HondaParts")));
        service.AddHostedService<DiscordBot>();
    })
    .UseSerilog()
    .Build();

Helps.LoadHelp();

host.Start();
//Test
static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
}

