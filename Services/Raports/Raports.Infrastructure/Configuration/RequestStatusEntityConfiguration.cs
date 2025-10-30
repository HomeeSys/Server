namespace Raports.Infrastructure.Configuration
{
    internal class RequestStatusEntityConfiguration : IEntityTypeConfiguration<RequestStatus>
    {
        public void Configure(EntityTypeBuilder<RequestStatus> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(b => b.ID).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).HasComment("For example: 'Generated', 'Pending', etc...");
            builder.Property(x => x.Description).HasComment("What is exactly happening here");
        }
    }
}
