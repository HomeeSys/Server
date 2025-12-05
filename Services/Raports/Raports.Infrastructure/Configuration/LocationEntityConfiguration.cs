namespace Raports.Infrastructure.Configuration;

public class LocationEntityConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasComment("For example: 'Kitchen', 'Attic' etc...");
        builder.Property(x => x.Hash).IsRequired();
    }
}
