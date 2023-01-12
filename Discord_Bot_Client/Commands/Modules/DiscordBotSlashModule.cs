using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_Client.Commands.Modules
{
    public class DiscordBotSlashModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IServiceProvider _service;

        public DiscordBotSlashModule(IServiceProvider service)
        {
            _service = service;
        }

        [SlashCommand("test","")]
        public async Task Crunchyroll()
        {
            await RespondAsync("Hello World");
        }
    }
}
