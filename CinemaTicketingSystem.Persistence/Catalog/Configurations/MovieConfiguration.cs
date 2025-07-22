using CinemaTicketingSystem.Domain.Catalog;
using CinemaTicketingSystem.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Catalog.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        // Table configuration
        builder.ToTable("Movies", "catalogs");

        // Primary key
        builder.HasKey(m => m.Id);

        // Properties
        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(MovieConst.TitleMaxLength);

        builder.Property(m => m.OriginalTitle)
            .HasMaxLength(MovieConst.OriginalTitleMaxLength);

        builder.Property(m => m.Description)
            .HasMaxLength(MovieConst.DescriptionMaxLength);

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
    }
}