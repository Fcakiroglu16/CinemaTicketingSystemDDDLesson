using CinemaTicketingSystem.Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Scheduling;

public class MovieSnapshotConfiguration : IEntityTypeConfiguration<MovieSnapshot>
{
    public void Configure(EntityTypeBuilder<MovieSnapshot> builder)
    {
        // Table configuration
        builder.ToTable("MovieSnapshot", "scheduling");

        // Primary key
        builder.HasKey(ms => ms.Id);

        // Properties
        builder.Property(ms => ms.Id)
            .ValueGeneratedNever();

        // Owned type for Duration (Value Object)
        builder.OwnsOne(m => m.Duration, duration =>
        {
            duration.Property(d => d.Minutes)
                .IsRequired()
                .HasColumnName("DurationMinutes");

            // Computed columns for convenience
            duration.Ignore(d => d.Hours);
            duration.Ignore(d => d.RemainingMinutes);
        });

        builder.Property(ms => ms.SupportedTechnology);
    }
}