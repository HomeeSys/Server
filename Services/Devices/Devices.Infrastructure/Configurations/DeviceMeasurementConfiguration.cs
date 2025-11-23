namespace Devices.Infrastructure.Configurations;

internal class DeviceMeasurementConfiguration : IEntityTypeConfiguration<DeviceMeasurementType>
{
    public void Configure(EntityTypeBuilder<DeviceMeasurementType> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
    }
}
