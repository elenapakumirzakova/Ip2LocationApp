namespace Ip2LocationApp.DataAccess;
public class AppDbContext : DbContext
{
    public DbSet<IpLocation> IpLocation { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Ip2Location");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IpLocation>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<IpLocation>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
    }
}