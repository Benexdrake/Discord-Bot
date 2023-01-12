namespace Discord_Scraper_Bot_Client.Data;
public class CrunchyrollDBContext : DbContext
{
    public DbSet<Anime> Animes { get; set; }

    public CrunchyrollDBContext(DbContextOptions<CrunchyrollDBContext> options) : base(options)
    {

    }
}
