using Discord_Bot.Embeds;
using Webscraper_API.Scraper.Steam.Models;

namespace Discord_Bot.Commands.Slash;

public class SteamSlashCommands
{
	private readonly ISteamService _ss;
	private readonly SteamEmbed _embed;
	public SteamSlashCommands(IServiceProvider service)
	{
		_ss = service.GetRequiredService<ISteamService>();
		_embed = service.GetRequiredService<SteamEmbed>();
	}

	public async Task Start(SocketSlashCommand arg)
	{
		//_ = Task.Run(async () =>
		//{
		//	Game game = new Game();			
		//	List<Game> games = new List<Game>();

  //          await arg.RespondAsync("Please wait");
  //          var message = arg.GetOriginalResponseAsync().Result as Discord.IUserMessage;

  //          var option = arg.Data.Options.FirstOrDefault();
  //          string value = option.Value.ToString();

		//	switch(option.Name)
		//	{
		//		case "game":

		//			break;
		//		case("update"):
					
		//			break;
		//	}

  //      });
	}
}
