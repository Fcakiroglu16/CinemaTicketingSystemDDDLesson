using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Ticketing.Configurations;

internal class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations", "Ticketing");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.ScheduledMovieShowId).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.ReservationTime).IsRequired();
        builder.Property(x => x.ExpirationTime).IsRequired();
        builder.Property(x => x.Status).IsRequired();


        builder.HasMany(x => x.ReservationSeatList).WithOne(y => y.Reservation);

        builder.Metadata.FindNavigation(nameof(Reservation.ReservationSeatList))!.SetField("_reservationSeatList");
    }
}