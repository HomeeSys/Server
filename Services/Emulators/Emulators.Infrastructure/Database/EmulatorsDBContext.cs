namespace Emulators.Infrastructure.Database;

public class EmulatorsDBContext : DbContext
{
    public EmulatorsDBContext(DbContextOptions<EmulatorsDBContext> options) : base(options) { }

    public DbSet<Device> Devices { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<ChartTemplate> ChartTemplates { get; set; }
    public DbSet<Sample> Samples { get; set; }
    public DbSet<ChartOffset> ChartOffsets { get; set; }
    public DbSet<Measurement> Measurements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
