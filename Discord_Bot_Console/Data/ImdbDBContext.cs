namespace Discord_Bot_Console.Data;

public class ImdbDBContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }

    public ImdbDBContext(DbContextOptions<ImdbDBContext> options) : base(options)
    {

    }
}
