using Discord;
using Discord_Bot.Interfaces.Services;

namespace Discord_Bot.Logic;

public class InsightDigitalLogic
{
	private readonly IInsightDigitalService _ids;	
	private readonly IID_API _api;
	public InsightDigitalLogic(IServiceProvider service)
	{
		_ids = service.GetRequiredService<IInsightDigitalService>();
		_api = service.GetRequiredService<IID_API>();
	}

	public async Task GetHandy(IUserMessage message,string param)
	{
		var split1 = param.Split(' ');
		string url = string.Empty;
		foreach (var item in split1)
		{
			if(item.Contains("https://www.inside-digital.de/handys/"))
				url = item;
		}
		if(!string.IsNullOrWhiteSpace(url))
		{
			message.ModifyAsync(x => x.Content= $"Looking now for {url}");
			await HandyByUrl(message, url,0,0);
			
        }
		else
            message.ModifyAsync(x => x.Content = "There was an Error");
	}

	public async Task GetHandys(IUserMessage message)
	{
        message.ModifyAsync(x => x.Content = $"Looking now for Urls");
		var WatchUrls = _api.GetHandyUrls();
		while(!WatchUrls.IsCompleted)
		{
            message.ModifyAsync(x => x.Content = _api.Message);
            await Task.Delay(1000);
		}
		var urls = WatchUrls.Result;
		int i = 0;
		foreach (var url in urls)
		{
            await HandyByUrl(message, url, i, urls.Length);
			i++;
        }
    }

	private async Task HandyByUrl(IUserMessage message ,string url, int n, int max)
	{
        var handy = _api.GetHandyAsync(url).Result;
        if (handy is not null)
        {
			if(n == 0)
				message.ModifyAsync(x => x.Content = $"Found {handy.Model}");
			else
                message.ModifyAsync(x => x.Content = $"Found {handy.Model}\n{Helper.Percent(n, max)}% / 100%\n{n} / {max}");
            await _ids.CreateOrUpdateAsync(handy);
        }
    }
}
