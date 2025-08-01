using CinemaTicketingSystem.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Catalog.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        // Table configuration
        builder.ToTable("Seats", "catalogs");

        // Primary key
        builder.HasKey(s => s.Id);

        // Properties
        builder.Property(s => s.Id)
            .ValueGeneratedNever();


        builder.OwnsOne(x => x.SeatPosition, seatBuilder =>
        {
            seatBuilder.Property(s => s.Number)
                .HasColumnName("Number")
                .IsRequired();

            seatBuilder.Property(s => s.Row)
                .HasColumnName("Row")
                .HasMaxLength(1).IsFixedLength()
                .IsRequired().IsUnicode(false);
        });


        // Enum configuration for SeatType
        builder.Property(s => s.Type).HasMaxLength(20);

        builder.Property(s => s.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);
    }
}