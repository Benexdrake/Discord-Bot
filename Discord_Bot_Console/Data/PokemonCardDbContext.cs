namespace Discord_Bot_Console.Data;

internal class PokemonCardDbContext : DbContext
{
    public DbSet<PokemonCard> PokemonCards { get; set; }
    public PokemonCardDbContext(DbContextOptions<PokemonCardDbContext> options) : base(options)
    {

    }
}
