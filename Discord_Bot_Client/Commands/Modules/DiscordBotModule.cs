using Discord.Interactions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Discord_Bot_Client.Commands.Modules
{
    public class DiscordBotModule : ModuleBase<SocketCommandContext>
    {
        private readonly IServiceProvider _service;
        private readonly IConfiguration _configuration;

        public DiscordBotModule(IServiceProvider service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }












        //[Command("IHKPDF", true)]
        //public async Task GetIHKRandomPDF([Remainder] string param)
        //{

        //    var message = await Context.Message.ReplyAsync($"Please wait, creating PDF for {param}");

        //    await _pdfSeperator.Start(_pdfPath);
        //    var file = await _pdfRandomizer.Start(param, _pdfPath, _pdfOutputPath);
        //    if (!string.IsNullOrWhiteSpace(file))
        //    {
        //        var e = new List<FileAttachment>();
        //        e.Add(new FileAttachment(file));
        //        await message.ModifyAsync(x => x.Content = "Here your PDF");
        //        await message.ModifyAsync(x => x.Attachments = e);
        //    }
        //    else
        //        Context.Channel.SendMessageAsync($"Cant make a Pdf for {param}, please check Config.json or the right param, like AP1,AP2, GA1_AE, GA1_SI or GA2");
        //}

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
