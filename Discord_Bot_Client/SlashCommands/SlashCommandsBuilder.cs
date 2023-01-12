using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_Client.SlashCommands
{
    public class SlashCommandsBuilder
    {
        private readonly IServiceProvider _service;
        private readonly DiscordSocketClient _botClient;

        public SlashCommandsBuilder(IServiceProvider service)
        {
            _service = service;
            _botClient = service.GetRequiredService<DiscordSocketClient>();
        }

        public async Task Initialization()
        {
            // Erstellen aller Slash Commands

            var options = new List<SlashCommandOptionBuilder>();

            for (int i = 0; i < 5; i++)
            {
                options.Add(new SlashCommandOptionBuilder().WithName($"a").WithDescription($"test").WithType(ApplicationCommandOptionType.String));
            }


            var cc = new SlashCommandBuilder();
            cc.WithName("crunchyroll")
            .WithDescription("this is a test yo")
            .AddOption(new SlashCommandOptionBuilder().WithName("name").WithDescription("search with name").WithType(ApplicationCommandOptionType.String))
            .AddOption(new SlashCommandOptionBuilder().WithName("url").WithDescription("search with url").WithType(ApplicationCommandOptionType.String))
            .AddOption(new SlashCommandOptionBuilder().WithName("rating").WithDescription("search with rating 0-5").WithType(ApplicationCommandOptionType.Number))
            .AddOption(new SlashCommandOptionBuilder().WithName("episodes").WithDescription("search with episodes count").WithType(ApplicationCommandOptionType.Integer))
            .AddOption(new SlashCommandOptionBuilder().WithName("publisher").WithDescription("search with publisher name").WithType(ApplicationCommandOptionType.Integer))
            .AddOption(new SlashCommandOptionBuilder().WithName("genre").WithDescription("choose what genre you want to see")
              .AddChoice("action", 1)
              .AddChoice("adventure", 2)
              .AddChoice("comedy", 3)
              .AddChoice("drama", 4)
              .AddChoice("fantasy", 5)
              .AddChoice("music", 6)
              .AddChoice("romance", 7)
              .AddChoice("science-fiction", 8)
              .AddChoice("seinen", 9)
              .AddChoice("shojo", 10)
              .AddChoice("shonen", 11)
              .AddChoice("slice-of-life", 12)
              .AddChoice("sport", 13)
              .AddChoice("supernatural", 14)
              .AddChoice("thriller", 15)
              .WithType(ApplicationCommandOptionType.Integer))
            .WithDefaultPermission(true);

            var test = new SlashCommandBuilder();
            test.Name= "test";
            test.Description= "testing some shit";

            await _botClient.Rest.CreateGuildCommand(cc.Build(), 998136328032112671);
            await _botClient.Rest.CreateGuildCommand(test.Build(), 998136328032112671);

        }
    }
}
