namespace Emulators.Infrastructure.Configurations;

internal class DeviceEntityConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.DeviceNumber).HasComment("This is unique name for device").IsRequired();
        builder.HasIndex(x => x.DeviceNumber).IsUnique();
        builder.Property(x => x.Spread).HasComment("Measurement value spread, expressed in percentage.");

        builder.ToTable(x =>
        {
            x.HasCheckConstraint("CK_Device_Spread_Range", "Spread >= 0 AND Spread <= 100");
        });
    }
}
