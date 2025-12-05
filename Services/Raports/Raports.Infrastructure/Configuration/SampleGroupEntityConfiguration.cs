namespace Raports.Infrastructure.Configuration;

internal class SampleGroupEntityConfiguration : IEntityTypeConfiguration<SampleGroup>
{
    public void Configure(EntityTypeBuilder<SampleGroup> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.Date).HasComment("Time of measurement");
        builder.Property(x => x.Value).HasComment("Value  of measurement");

        builder.HasOne(x => x.LocationGroup)
            .WithMany(x => x.SampleGroups)
            .HasForeignKey(x => x.LocationGroupID)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
