namespace Raports.Infrastructure.Configuration;

internal class RequestedMeasurementEntityConfig : IEntityTypeConfiguration<RequestedMeasurement>
{
    public void Configure(EntityTypeBuilder<RequestedMeasurement> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
    }
}
