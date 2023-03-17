namespace Scraper_Bot.Commands.Slash;

public class ToolsSlashCommands
{
    private readonly ISteamService _ss;
    private readonly SteamEmbed _embed;
    private readonly Amazon_API _amazon_API;
    public ToolsSlashCommands(IServiceProvider service)
    {
        _ss = service.GetRequiredService<ISteamService>();
        _embed= service.GetRequiredService<SteamEmbed>();
        _amazon_API = service.GetRequiredService<Amazon_API>();
    }

    public async Task LFG(SocketSlashCommand arg, SocketForumChannel channel)
    {
        try
        {
            await arg.RespondAsync("Please wait");
            var message = arg.GetOriginalResponseAsync().Result as IUserMessage;

            string url = string.Empty;
            string description = string.Empty;

            foreach (var option in arg.Data.Options) 
            {
                if(option.Name.Equals("url"))
                    url = option.Value.ToString();
                if(option.Name.Equals("description"))
                    description= option.Value.ToString();
            }
            
            if(url.Contains("https://store.steampowered.com/app/"))
            {
                var game = _ss.GetSteamGame(url).Result;
                if(!string.IsNullOrWhiteSpace(game.Title))
                {
                    ForumTagBuilder tagBuilder = new ForumTagBuilder();
                    var tag = tagBuilder.WithName("Test").Build();

                    var embed = _embed.GameEmbed(game).Result;
                    var post = await channel.CreatePostAsync(game.Title,text: arg.User.Mention);
                    post.SendMessageAsync(embed: embed.Build());
                    
                    await post.AddUserAsync(arg.User as IGuildUser);
                }
            }
            await message.DeleteAsync();
        }
        catch (Exception err)
        {
            Log.Logger.Error(err.Message);
        }
    }

    public async Task Purge(SocketSlashCommand arg, SocketTextChannel channel)
    {
        await arg.RespondAsync("Please wait");

        await _amazon_API.GetProduct("https://www.amazon.de/Warner-Bros-Entertainment-Hogwarts-Nintendo/dp/B0BBTPJ9R4");

        var message = arg.GetOriginalResponseAsync().Result as IUserMessage;

        var option = arg.Data.Options.FirstOrDefault();
        string value = option.Value.ToString();

        int kill = 0;

        if (value.ToLower().Equals("all"))
        {
            kill = int.MaxValue;
        }
        else
        {
            bool isNumber = int.TryParse(value, out kill);
        }
        await DeleteMessages(kill, channel);
    }



    private async Task DeleteMessages(int n, SocketTextChannel channel)
    {
        var messages = channel.GetMessagesAsync(n).Flatten();
        foreach (var h in await messages.ToArrayAsync())
        {
            await h.DeleteAsync();
        }
    }




}
