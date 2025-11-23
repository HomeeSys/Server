namespace Emulators.Infrastructure.Configurations;

internal class ChartTemplateEntityConfiguration : IEntityTypeConfiguration<ChartTemplate>
{
    public void Configure(EntityTypeBuilder<ChartTemplate> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();

        //  One 'ChartTemplate' has one 'Measurement' type.
        builder.HasOne(x => x.Measurement)
            .WithMany()
            .HasForeignKey(x => x.MeasurementID)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
