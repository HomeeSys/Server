namespace Raports.Infrastructure.Configuration;

internal class RequestEntityConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();

        builder.Property(x => x.StartDate).HasComment("Raport will be generate from this date");
        builder.Property(x => x.EndDate).HasComment("Raport will be generate to this date");
        builder.Property(x => x.RequestCreationDate).HasComment("Date when request was created");

        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => x.StatusID)
            .IsRequired();

        builder.HasOne(x => x.Period)
            .WithMany()
            .HasForeignKey(x => x.PeriodID)
            .IsRequired();
    }
}
