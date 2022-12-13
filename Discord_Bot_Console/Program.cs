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
        service.AddSingleton<IDiscordBotModule, DiscordBotModule>();
        service.AddSingleton<DiscordEmbedBuilder>();
        service.AddScoped<IGiphyController, GiphyController>();
        service.AddSingleton<Browser>();
        service.AddTransient<ICR_API, CR_API>();
        service.AddTransient<IIMDb_API, IMDb_API>();
        service.AddTransient<IPokemon_API, Pokemon_API>();
        service.AddTransient<ITCG_API, TCG_API>();
        service.AddTransient<IHonda_Api, Honda_Api>();

        service.AddScoped<PdfSeperator>();
        service.AddScoped<PdfRandomizer>();



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
        .AddJsonFile("secrets.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
}

