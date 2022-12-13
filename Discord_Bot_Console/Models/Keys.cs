using Discord_Bot_Console.Interfaces;

namespace Discord_Bot_Console.Models
{
    public class Keys : IKeys
    {
        public string ApplicationID { get; set; }
        public string PublicKey { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string Token { get; set; }
        public string Giphy { get; set; }

        private readonly string discordKeys;

        public Keys(IConfiguration config)
        {
            discordKeys = config.GetValue<string>("DiscordKeys");
            LoadKeys(config);
        }
        public Keys()
        {

        }

        //public Keys(string filePath)
        //{
        //    discordKeys= filePath;
        //    LoadKeys();
        //}
        //
        private void LoadKeys(IConfiguration config)
        {
            ApplicationID = config["DiscordKeys:ApplicationID"];
            PublicKey = config["DiscordKeys:PublicKey"];
            ClientID = config["DiscordKeys:ClientID"];
            ClientSecret = config["DiscordKeys:ClientSecret"];
            Token = config["DiscordKeys:Token"];
            Giphy = config["DiscordKeys:Giphy"];
            


            //using (StreamReader r = new StreamReader(discordKeys))
            //{
            //    string json = r.ReadToEnd();
            //    var key = JsonConvert.DeserializeObject<Keys>(json);
            //    ApplicationID = key.ApplicationID;
            //    PublicKey = key.PublicKey;
            //    ClientID = key.ClientID;
            //    ClientSecret = key.ClientSecret;
            //    Token = key.Token;
            //    Giphy = key.Giphy;
            //}
        }
    }
}
