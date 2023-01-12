namespace Discord_Scraper_Bot_Client.Data;

internal class PokemonCardDbContext : DbContext
{
    public DbSet<PokemonCard> PokemonCards { get; set; }
    public PokemonCardDbContext(DbContextOptions<PokemonCardDbContext> options) : base(options)
    {

    }
}
