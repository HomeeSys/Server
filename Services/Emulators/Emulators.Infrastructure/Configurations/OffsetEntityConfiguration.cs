namespace Emulators.Infrastructure.Configurations;

internal class OffsetEntityConfiguration : IEntityTypeConfiguration<ChartOffset>
{
    public void Configure(EntityTypeBuilder<ChartOffset> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.Time).IsRequired();
        builder.Property(x => x.Value).IsRequired();

        //  1:N
        builder.HasOne(x => x.ChartTemplate)
            .WithMany()
            .HasForeignKey(x => x.ChartTemplateID)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        //  1:N
        builder.HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationID)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
