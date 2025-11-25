namespace Raports.Infrastructure.Configuration;

internal class PeriodEntityConfiguration : IEntityTypeConfiguration<Period>
{
    public void Configure(EntityTypeBuilder<Period> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.MaxAcceptableMissingTimeFrame).HasDefaultValue(1);
        builder.Property(x => x.TimeFrame).HasDefaultValue(TimeSpan.Zero);
        builder.Property(x => x.Name).HasComment("For example: 'Daily', 'Weekly', 'Monthly', etc...");

        builder.ToTable(x =>
        {
            x.HasCheckConstraint("CK_Period_MaxAcceptableMissingTimeFrame_Range", "MaxAcceptableMissingTimeFrame >= 1 AND MaxAcceptableMissingTimeFrame <= 100");
        });
    }
}
