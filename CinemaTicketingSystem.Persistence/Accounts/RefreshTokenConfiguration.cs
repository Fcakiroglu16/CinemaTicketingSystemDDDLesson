using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;
using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Accounts;

internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {

        builder.ToTable("RefreshTokens", "accounts");
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId).ValueGeneratedNever();
        builder.Property(x => x.UserId)
            .HasConversion(
                userId => userId.Value,
                value => new UserId(value)
            )
            .ValueGeneratedNever();
        builder.Property(x => x.Expiration).IsRequired();
        builder.HasIndex(x => x.Token).IsUnique();
        builder.Property(x => x.Token).HasMaxLength(36).IsRequired();
    }
}