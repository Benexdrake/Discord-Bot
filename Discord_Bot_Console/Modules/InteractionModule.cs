using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_Console.Modules
{
    public class InteractionModule : InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("test123", "This is a Test")]
        public async Task Test()
        {
            await RespondAsync("Hello");
        }

    }
}
