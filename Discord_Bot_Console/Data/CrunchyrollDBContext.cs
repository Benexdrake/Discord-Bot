namespace Discord_Bot_Console.Data;
public class CrunchyrollDBContext : DbContext
{
    public DbSet<Anime> Animes { get; set; }

    public CrunchyrollDBContext(DbContextOptions<CrunchyrollDBContext> options) : base(options)
    {

    }
}
