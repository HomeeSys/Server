namespace Raports.Infrastructure.Configuration;

internal class MeasurementGroupEntityConfiguration : IEntityTypeConfiguration<MeasurementGroup>
{
    public void Configure(EntityTypeBuilder<MeasurementGroup> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();

        builder.Property(x => x.Summary).HasComment("Combined summary for all location groups");

        builder.HasOne(x => x.Raport)
            .WithMany(x => x.MeasurementGroups)
            .HasForeignKey(x => x.RaportID)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
