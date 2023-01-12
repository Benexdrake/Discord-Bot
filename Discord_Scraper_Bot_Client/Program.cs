using Discord_Scraper_Bot_Client;

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
        service.AddSingleton<Browser>();
        service.AddScoped<ICR_API, CR_API>();
        service.AddScoped<IIMDb_API, IMDb_API>();
        service.AddScoped<IPokemon_API, Pokemon_API>();
        service.AddScoped<ITCG_API, TCG_API>();
        service.AddScoped<IHonda_Api, Honda_Api>();
        service.AddScoped<ITCG_M_API, TCG_M_API>();



        service.AddSingleton<DiscordSocketClient>();
        service.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
        service.AddSingleton<CommandHandler>();


        service.AddDbContext<CrunchyrollDBContext>(options => options.UseSqlServer(conf.GetConnectionString("Crunchyroll")));
        service.AddDbContext<ImdbDBContext>(options => options.UseSqlServer(conf.GetConnectionString("IMDB")));
        service.AddDbContext<PokemonDBContext>(options => options.UseSqlServer(conf.GetConnectionString("Pokemon")));
        service.AddDbContext<PokemonCardDbContext>(options => options.UseSqlServer(conf.GetConnectionString("PokemonCards")));
        service.AddDbContext<HondaPartsDbContext>(options => options.UseSqlServer(conf.GetConnectionString("HondaParts")));
        service.AddHostedService<Bot>();
    })
    .UseSerilog()
    .Build();



await host.StartAsync();



static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
}