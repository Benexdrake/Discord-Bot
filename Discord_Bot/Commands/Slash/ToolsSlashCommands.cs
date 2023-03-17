using Discord;

namespace Discord_Bot.Commands.Slash;

public class ToolsSlashCommands
{
	public ToolsSlashCommands(IServiceProvider service)
	{

	}

	public async Task Purge(SocketSlashCommand arg, SocketTextChannel channel)
	{
        await arg.RespondAsync("Please wait");
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
        await DeleteMessages(kill,channel);
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
