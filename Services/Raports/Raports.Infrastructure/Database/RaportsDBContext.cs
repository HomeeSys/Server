namespace Raports.Infrastructure.Database
{
    public class RaportsDBContext : DbContext
    {
        public RaportsDBContext(DbContextOptions<RaportsDBContext> options) : base(options) { }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<MeasurementGroup> MeasurementGroups { get; set; }
        public DbSet<LocationGroup> LocationGroups { get; set; }
        public DbSet<SampleGroup> SampleGroups { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Raport> Raports { get; set; }
        public DbSet<RequestedLocation> RequestedLocations { get; set; }
        public DbSet<RequestedMeasurement> RequestedMeasurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}