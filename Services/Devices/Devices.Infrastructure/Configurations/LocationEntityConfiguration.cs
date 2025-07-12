namespace Devices.Infrastructure.Configurations
{
    public class LocationEntityConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            //  DeviceNumber
            builder.Property(x => x.Name).HasComment("This is unique name for device").IsRequired();

            //  One to many mapping
            builder.HasMany<Device>().WithOne(x=>x.Location).HasForeignKey(x => x.LocationId).IsRequired();
        }
    }
}
