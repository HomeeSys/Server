namespace Devices.Infrastructure.Configurations;

internal class MeasurementTypeConfiguration : IEntityTypeConfiguration<MeasurementType>
{
    public void Configure(EntityTypeBuilder<MeasurementType> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasComment("For example: 'Air Temperature'").IsRequired();
        builder.Property(x => x.Unit).HasComment("For example: 'ppm'").IsRequired();
    }
}
