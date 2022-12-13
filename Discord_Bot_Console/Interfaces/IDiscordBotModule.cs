namespace Discord_Bot_Console.Interfaces
{
    public interface IDiscordBotModule
    {
        Task DeleteMessages(int n);
        Task Gif([Remainder] string param);
        Task Hallo();
        Task Hallo(string param);
        Task Helping();
        Task Helping(string param);
        Task Purge(string n);
    }
}