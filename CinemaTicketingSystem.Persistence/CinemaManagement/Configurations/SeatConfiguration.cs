using CinemaTicketingSystem.Domain.CinemaManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.CinemaManagment.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        // Table configuration
        builder.ToTable("Seats", "cinema_mgmt");

        // Primary key
        builder.HasKey(s => s.Id);

        // Properties
        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.Row)
            .HasMaxLength(1).IsFixedLength()
            .IsRequired().IsUnicode(false);

        builder.Property(s => s.Number)
            .IsRequired();

        // Enum configuration for SeatType
        builder.Property(s => s.Type).HasMaxLength(20);

        builder.Property(s => s.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);




    }
}