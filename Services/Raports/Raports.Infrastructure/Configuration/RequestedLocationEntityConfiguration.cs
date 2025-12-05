namespace Raports.Infrastructure.Configuration;

internal class RequestedLocationEntityConfiguration : IEntityTypeConfiguration<RequestedLocation>
{
    public void Configure(EntityTypeBuilder<RequestedLocation> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
    }
}
