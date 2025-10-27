namespace Raports.Infrastructure.Database
{
    public class RaportsDBContext : DbContext
    {
        public RaportsDBContext(DbContextOptions<RaportsDBContext> options) : base(options) { }

        public DbSet<Request> Requests { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<Raport> Raports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}