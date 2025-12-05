namespace Raports.Infrastructure.Configuration;

internal class LocationGroupEntityConfiguration : IEntityTypeConfiguration<LocationGroup>
{
    public void Configure(EntityTypeBuilder<LocationGroup> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();

        builder.Property(x => x.Summary).HasComment("Verbal summary of data stored for this location");

        builder.HasOne(x => x.MeasurementGroup)
            .WithMany(x => x.LocationGroups)
            .HasForeignKey(x => x.MeasurementGroupID)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
