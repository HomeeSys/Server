namespace Devices.Infrastructure.Configurations
{
    public class DeviceEntityConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            //  Let database generate ID for me.
            builder.HasKey(x => x.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.RegisterDate);
            builder.Property(x => x.Name);

            //  DeviceNumber
            builder.Property(x => x.DeviceNumber).HasComment("This is unique name for device").IsRequired();
            builder.HasIndex(x => x.DeviceNumber).IsUnique();

            //  This is mapping for 1:n relationship defined as code instead of convention.
            builder.HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .IsRequired();

            builder.HasOne(x => x.TimestampConfiguration)
                .WithMany()
                .HasForeignKey(x => x.TimestampConfigurationId)
                .IsRequired();

            builder.HasOne(x => x.MeasurementConfiguration)
                .WithOne(x => x.Device)
                .HasForeignKey<Device>(x => x.MeasurementConfigId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
