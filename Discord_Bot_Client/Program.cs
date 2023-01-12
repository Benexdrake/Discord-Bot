using Discord_Bot_Client;

IConfiguration conf;

var builder = new ConfigurationBuilder();
Config.BuildConfig(builder);
Log.Logger = Config.GetLogger(builder);

conf= builder.Build();

var host = Config.CreateHost(conf).Result
    .UseSerilog()
    .Build();

await host.StartAsync();