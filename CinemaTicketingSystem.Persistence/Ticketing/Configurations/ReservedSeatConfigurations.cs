using CinemaTicketingSystem.Domain.Ticketing.Reservations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Ticketing.Configurations
{
    internal class ReservedSeatConfigurations : IEntityTypeConfiguration<ReservedSeat>
    {
        public void Configure(EntityTypeBuilder<ReservedSeat> builder)
        {

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne(x => x.SeatReservation).WithMany(x => x.ReservedSeats).HasForeignKey();
            builder.OwnsOne(x => x.SeatNumber);
        }
    }
}
