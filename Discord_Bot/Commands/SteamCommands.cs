using Discord;
using Discord.Commands;
using Discord_Bot.Interfaces.Services;
using Webscraper_API;

namespace Discord_Bot.Commands;

public class SteamCommands : ModuleBase<SocketCommandContext>
{
    private readonly ISteamService _ss;
	private readonly Browser _browser;
	private readonly ISteam_Api _api;

	public SteamCommands(IServiceProvider service)
	{
		_ss = service.GetRequiredService<ISteamService>();
		_browser= service.GetRequiredService<Browser>();
		_api = service.GetRequiredService<ISteam_Api>();
	}

	[Command("Steam")]
	public async Task GetSteamGame(string url)
	{
        var message = Context.Message.ReplyAsync("Please wait!").Result;

		var game = _ss.GetSteamGame(url).Result;
		if(string.IsNullOrWhiteSpace(game.Id))
		{
			await message.ModifyAsync(x => x.Content = "Please wait, scraping the Game from Steam");
			_browser.FirefoxDebug();
			game = _api.GetSteamGame(url).Result;
			if(game is not null)
			{
				await _ss.CreateOrUpdate(game);
				await message.ModifyAsync(x => x.Content = $"Found: {game.Title}");
			}
			else
                message.ModifyAsync(x => x.Content = "Sorry, dont found an Game.");
		}
		else
            await message.ModifyAsync(x => x.Content = $"Found: {game.Title}");
    }
}
