namespace Raports.Infrastructure.Configuration;

internal class PeriodEntityConfiguration : IEntityTypeConfiguration<Period>
{
    public void Configure(EntityTypeBuilder<Period> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(b => b.ID).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).HasComment("For example: 'Daily', 'Weekly', 'Monthly', etc...");
    }
}
