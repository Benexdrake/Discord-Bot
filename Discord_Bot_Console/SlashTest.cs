using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_Console
{
    public class SlashTest : InteractionModuleBase<SocketInteractionContext>
    {
        public SlashTest()
        {

        }

        [SlashCommand("test","Hey Ho, Lets a go...")]
        public async Task Testing()
        {
            Console.WriteLine("Hello World");
        }
    }
}
