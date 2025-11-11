namespace Emulators.Infrastructure.Configurations;

internal class SampleEntityConfiguration : IEntityTypeConfiguration<Sample>
{
    public void Configure(EntityTypeBuilder<Sample> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.Time).IsRequired();
        builder.Property(x => x.Value).IsRequired();

        //  1:N
        builder.HasOne(x => x.ChartTemplate)
            .WithMany(ct => ct.Samples)
            .HasForeignKey(x => x.ChartTemplateID)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
