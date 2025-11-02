namespace Devices.Infrastructure.Configurations;

public class LocationEntityConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasComment("Name of location, for example: 'Kitchen'...").IsRequired();
    }
}
