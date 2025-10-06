namespace Devices.Infrastructure.Database
{
    public class DevicesDBContext : DbContext
    {
        public DevicesDBContext(DbContextOptions<DevicesDBContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TimestampConfiguration> TimestampConfigurations { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<MeasurementConfig> MeasurementConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  This allows us to scan all of assembly of this project in search for any class that implements
            //  `IEntityTypeConfiguration` interface. Then it applies config stored in those classes into db schema.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}