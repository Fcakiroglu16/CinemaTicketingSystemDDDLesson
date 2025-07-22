using CinemaTicketingSystem.Domain.Catalog;
using CinemaTicketingSystem.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Catalog.Configurations;

public class CinemaHallConfiguration : IEntityTypeConfiguration<CinemaHall>
{
    public void Configure(EntityTypeBuilder<CinemaHall> builder)
    {
        // Table configuration
        builder.ToTable("CinemaHalls", "catalogs");

        // Primary key
        builder.HasKey(h => h.Id);

        // Properties
        builder.Property(h => h.Id)
            .ValueGeneratedNever();

        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.IsOperational)
            .IsRequired()
            .HasDefaultValue(true);

        // Enum configuration for HallTechnology (Flags enum)
        builder.Property(h => h.SupportedTechnologies)
            .HasConversion<int>()
            .HasDefaultValue(ScreeningTechnology.Standard);

        // Foreign key to Cinema
        builder.Property<Guid>("CinemaId")
            .IsRequired();


        // Relationships
        builder.HasMany(x => x.Seats).WithOne(x => x.CinemaHall);

        builder.Metadata.FindNavigation(nameof(CinemaHall.Seats))!.SetField("seats");
    }
}