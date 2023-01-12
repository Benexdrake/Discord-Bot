namespace Discord_Bot_Client.Data;

public class HondaPartsDbContext : DbContext
{
    public DbSet<NewParts> HondaParts { get; set; }
    //public DbSet<Accessories> Accessories { get; set; }
    //public DbSet<PartFits> PartFits { get; set; }
    //public DbSet<PartFitment> PartFitments { get; set; }
    public HondaPartsDbContext(DbContextOptions<HondaPartsDbContext> options) : base(options)
    {

    }
}
