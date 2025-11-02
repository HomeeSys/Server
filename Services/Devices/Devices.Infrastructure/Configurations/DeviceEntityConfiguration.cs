namespace Devices.Infrastructure.Configurations;

public class DeviceEntityConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.RegisterDate);
        builder.Property(x => x.Name);
        builder.Property(x => x.DeviceNumber).HasComment("This is unique name for device").IsRequired();
        builder.HasIndex(x => x.DeviceNumber).IsUnique();

        //  1:n
        builder.HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationID)
            .IsRequired();

        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => x.StatusID)
            .IsRequired();

        builder.HasOne(x => x.TimestampConfiguration)
            .WithMany()
            .HasForeignKey(x => x.TimestampConfigurationID)
            .IsRequired();

        //  1:1
        builder.HasOne(x => x.MeasurementConfiguration)
            .WithOne(x => x.Device)
            .HasForeignKey<Device>(x => x.MeasurementConfigurationID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
