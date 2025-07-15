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

            builder.OwnsOne(x => x.SeatNumber, seatBuilder =>
            {
                seatBuilder.Property(s => s.Number)
                    .HasColumnName("Number")
                    .IsRequired();

                seatBuilder.Property(s => s.Row)
                    .HasColumnName("Row")
                    .HasMaxLength(1).IsFixedLength()
                    .IsRequired().IsUnicode(false);
            });
        }
    }
}
