namespace Devices.Infrastructure.Configurations
{
    public class TimestampConfigurationConfiguration : IEntityTypeConfiguration<TimestampConfiguration>
    {
        public void Configure(EntityTypeBuilder<TimestampConfiguration> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();

            //  DeviceNumber
            builder.Property(x => x.Cron).HasComment("Timestamp measurement configuration stored in CRON format").IsRequired();

            //  One to many mapping
            builder.HasMany<Device>().WithOne(x => x.TimestampConfiguration).HasForeignKey(x => x.TimestampConfigurationId).IsRequired();
        }
    }
}
