namespace Raports.Infrastructure.Configuration;

internal class RaportEntityConfiguration : IEntityTypeConfiguration<Raport>
{
    public void Configure(EntityTypeBuilder<Raport> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();

        builder.Property(x => x.RaportCreationDate).HasComment("Date of Raport creation");
        builder.Property(x => x.RaportCompletedDate).HasComment("Date of Raport completion");
        builder.Property(x => x.StartDate).HasComment("Date of first measurement");
        builder.Property(x => x.EndDate).HasComment("Date of last measurement");
        builder.Property(x => x.DocumentHash).HasDefaultValue(Guid.Empty).HasComment("Hash that allows to identify PDF in Azure Blob Storage");

        builder.HasOne(x => x.Period)
            .WithMany()
            .HasForeignKey(x => x.PeriodID)
            .IsRequired();

        builder.HasMany(x => x.RequestedLocations)
            .WithMany()
            .UsingEntity<RequestedLocation>();

        builder.HasMany(x => x.RequestedMeasurements)
            .WithMany()
            .UsingEntity<RequestedMeasurement>();
    }
}
