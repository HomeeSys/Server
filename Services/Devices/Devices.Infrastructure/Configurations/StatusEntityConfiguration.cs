namespace Devices.Infrastructure.Configurations
{
    public class StatusEntityConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Type).IsRequired();

            builder.HasMany<Device>().WithOne(x => x.Status).HasForeignKey(x => x.StatusId).IsRequired();
        }
    }
}
