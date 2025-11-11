namespace Devices.Infrastructure.Configurations;

public class TimestampConfigurationConfiguration : IEntityTypeConfiguration<Timestamp>
{
    public void Configure(EntityTypeBuilder<Timestamp> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.Cron).HasComment("Timestamp measurement configuration stored in CRON format").IsRequired();
    }
}
