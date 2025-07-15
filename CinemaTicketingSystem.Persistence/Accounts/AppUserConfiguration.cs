using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Accounts
{
    internal class AppUserConfiguration : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode();

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode();
        }
    }
}
