using Discord.Interactions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Discord_Bot_Console.Controllers
{
    public class DiscordBotModule : ModuleBase<SocketCommandContext>, IDiscordBotModule
    {
        private readonly IGiphyController _g;
        private readonly PdfSeperator _pdfSeperator;
        private readonly PdfRandomizer _pdfRandomizer;
        private readonly Random rand;
        private readonly string _pdfPath;
        private readonly string _pdfOutputPath;

        public DiscordBotModule(IServiceProvider service, IConfiguration configuration)
        {
            _g = service.GetService<IGiphyController>();
            _pdfSeperator = service.GetRequiredService<PdfSeperator>();
            _pdfRandomizer = service.GetRequiredService<PdfRandomizer>();
            _pdfPath = configuration["IHKPDF:PDFPath"];
            _pdfOutputPath = configuration["IHKPDF:PDFSavePath"];
            rand = new Random();
        }

        [Command("hallo")]
        [Alias("hi")]
        public async Task Hallo()
        {
            string gifURL = _g.SearchGifs("Hallo").Result.Data.Url;
            await Context.Channel.DeleteMessageAsync(Context.Message);
            await Context.Channel.SendMessageAsync(gifURL);
            Console.WriteLine();
        }

        [Command("hallo")]
        [Alias("hi")]
        public async Task Hallo(string param)
        {
            string user = Context.Message.Author.Mention;
            string gifURL = _g.SearchGifs("Hallo").Result.Data.Url;
            await Context.Channel.DeleteMessageAsync(Context.Message);
            await Context.Channel.SendMessageAsync($"Hallo {param}");
            await Context.Channel.SendMessageAsync(gifURL);
        }

        [Command("gif", true)]
        [Alias("g")]
        public async Task Gif([Remainder] string param)
        {
            var gif = await _g.SearchGifs(param);

            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();
            var embed = await discordEmbedBuilder.GifEmbed(gif);

            if (gif is not null)
            {
                await DeleteMessages(1);
                await Context.Channel.SendMessageAsync(embed: embed.Build());
            }
        }

        [Command("IHKPDF", true)]
        public async Task GetIHKRandomPDF([Remainder] string param)
        {
            
            var message = await Context.Message.ReplyAsync($"Please wait, creating PDF for {param}");
            
            await _pdfSeperator.Start(_pdfPath);
            var file = await _pdfRandomizer.Start(param,_pdfPath,_pdfOutputPath);
            if (!string.IsNullOrWhiteSpace(file))
            {
                var e = new List<FileAttachment>();
                e.Add(new FileAttachment(file));
                await message.ModifyAsync(x => x.Content = "Here your PDF");
                await message.ModifyAsync(x => x.Attachments = e);
            }
            else
                Context.Channel.SendMessageAsync($"Cant make a Pdf for {param}, please check Config.json or the right param, like AP1,AP2, GA1_AE, GA1_SI or GA2");
        }

        [Command("help")]
        [Alias("?")]
        public async Task Helping()
        {
            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();
            var embed = await discordEmbedBuilder.HelpListEmbed(Helps.Helpings);

           
            await Context.Message.ReplyAsync(embed: embed.Build());
        }

        [Command("help")]
        [Alias("?")]
        public async Task Helping(string param)
        {
            DiscordEmbedBuilder discordEmbedBuilder = new DiscordEmbedBuilder();
            var h = Helps.Helpings.Where(x => x.CommandName.Contains(param)).FirstOrDefault();
            if (h is not null)
            {
                var embed = await discordEmbedBuilder.HelpEmbed(h);
                await Context.Message.ReplyAsync(embed: embed.Build());
            }
        }

        //Löscht Nachrichten
        [Command("purge")]
        public async Task Purge(string n)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message);

            bool isNumber = int.TryParse(n, out int count);
            if (isNumber)
            {
                await Context.Message.ReplyAsync($"Channel {Context.Channel.Name} wird nun gepurged!");
                await DeleteMessages(count);
            }
            else
            {
                if (n.ToLower().Equals("all"))
                {
                    count = int.MaxValue;
                    await Context.Channel.SendMessageAsync($"Channel {Context.Channel.Name} wird nun gepurged!");
                    await Task.Delay(5000);
                    await DeleteMessages(count);
                }
                else
                {
                    //await Context.Channel.SendMessageAsync(SearchCommands("purge").ErrorMessage);
                    await Task.Delay(5000);
                    await DeleteMessages(1);
                }
            }
        }

        public async Task DeleteMessages(int n)
        {
            var messages = Context.Channel.GetMessagesAsync(n).Flatten();
            foreach (var h in await messages.ToArrayAsync())
            {
                await Context.Channel.DeleteMessageAsync(h);
            }
        }
    }
}
