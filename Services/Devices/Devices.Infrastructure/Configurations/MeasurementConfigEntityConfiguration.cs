namespace Devices.Infrastructure.Configurations;

public class MeasurementConfigEntityConfiguration : IEntityTypeConfiguration<MeasurementConfiguration>
{
    public void Configure(EntityTypeBuilder<MeasurementConfiguration> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(x => x.ID).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Device)
               .WithOne(d => d.MeasurementConfiguration)
               .HasForeignKey<MeasurementConfiguration>(x => x.DeviceID)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
