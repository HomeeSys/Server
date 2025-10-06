namespace Devices.Infrastructure.Configurations;

public class MeasurementConfigEntityConfiguration : IEntityTypeConfiguration<MeasurementConfig>
{
    public void Configure(EntityTypeBuilder<MeasurementConfig> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Device)
               .WithOne(d => d.MeasurementConfiguration)
               .HasForeignKey<MeasurementConfig>(x => x.DeviceId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
