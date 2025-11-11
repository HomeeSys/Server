namespace Emulators.Infrastructure.Configurations;

internal class MeasurementEntityConfiguration : IEntityTypeConfiguration<Measurement>
{
    public void Configure(EntityTypeBuilder<Measurement> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(b => b.Name).HasComment("Name of measurement, for example 'Air Temperature'.");
        builder.Property(x => x.Unit).HasComment("Unit");
    }
}