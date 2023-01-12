using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webscraper_API.Scraper.TCG_Magic.Model;

namespace Discord_Bot_Console.Modules;

public class MagicModule : ModuleBase<SocketCommandContext>
{
	private readonly Browser _browser;
	private readonly ITCG_M_API _api;
	public MagicModule(IServiceProvider service)
	{
		_browser = service.GetRequiredService<Browser>();
		_api = service.GetRequiredService<ITCG_M_API>();
	}

	[Command("MagicCardsAll")]
	public async Task GetAllMagicCards()
	{
        string url = "https://scryfall.com/sets";
        List<Card> cards = new List<Card>();
        List<CardUrl> urls = new();

		var message = Context.Message.ReplyAsync("Please wait, looking for Set Urls").Result;

        
        var seturls = await _api.GetAllSetUrls();

        await message.ModifyAsync(x => x.Content = $"Found {seturls.Length} Sets, looking now for all Cards and there Urls, please wait...");
        await Task.Delay(3000);
        int i = 1;

        List<Card> saved = new();

        if (File.Exists("MagicCards.json"))
        {
            var read = File.ReadAllText("MagicCards.json");
            saved = JsonConvert.DeserializeObject<List<Card>>(read);
            cards.AddRange(saved);
        }

        foreach (var set in seturls.Reverse())
        {
            Log.Information(set);
            var cardUrls = await _api.GetAllCardBySetUrl(set);
            urls.AddRange(cardUrls);
            //await message.ModifyAsync(x => x.Content = $"{Helper.Percent(i, seturls.Length)}% / 100% {set}");
            await message.ModifyAsync(x => x.Content = $"Looking at {i} / {seturls.Length} - {set}");
            int j = 1;
            foreach (var u in cardUrls)
            {
                var c = saved.Where(x => x.Id.Equals(u.Id)).FirstOrDefault();

                if (c is null)
                {
                    await message.ModifyAsync(x => x.Content = $" {i}/{seturls.Length} - {Helper.Percent(j, cardUrls.Length)}% / 100% https://api.scryfall.com/cards/{u.Id}");
                    var card = _api.GetCard($"https://api.scryfall.com/cards/{u.Id}?format=json&pretty=true").Result;
                    cards.Add(card);
                }
                j++;
                
            }
            i++;
            var json = JsonConvert.SerializeObject(cards, Formatting.Indented);
            File.WriteAllText("MagicCards.json", json);
        }

        await message.ModifyAsync(x => x.Content = "Done here is your File");
    }


}
