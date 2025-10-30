namespace Raports.Infrastructure.Configuration
{
    internal class RaportEntityConfiguration : IEntityTypeConfiguration<Raport>
    {
        public void Configure(EntityTypeBuilder<Raport> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(b => b.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.CreationDate).HasComment("Date when raport was created");

            builder.HasOne(x => x.Request)
                .WithOne(r => r.Raport)
                .HasForeignKey<Raport>(x => x.RequestID)
                .IsRequired(false);

            builder.HasOne(x => x.Period)
                .WithMany()
                .HasForeignKey(x => x.PeriodID)
                .IsRequired();
        }
    }
}
