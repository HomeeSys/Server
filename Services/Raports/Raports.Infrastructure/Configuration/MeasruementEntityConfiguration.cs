namespace Raports.Infrastructure.Configuration;

internal class MeasruementEntityConfiguration : IEntityTypeConfiguration<Measurement>
{
    public void Configure(EntityTypeBuilder<Measurement> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.Name);
    }
}
