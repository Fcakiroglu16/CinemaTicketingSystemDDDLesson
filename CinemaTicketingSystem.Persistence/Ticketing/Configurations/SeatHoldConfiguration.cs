#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Ticketing.Configurations;

internal class SeatHoldConfiguration : IEntityTypeConfiguration<SeatHold>
{
    public void Configure(EntityTypeBuilder<SeatHold> builder)
    {
        builder.ToTable("SeatHolds", "Ticketing");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();


        builder.Property(x => x.ScheduledMovieShowId).IsRequired();
        builder.Property(x => x.ScreeningDate).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.OwnsOne(x => x.SeatPosition, seatBuilder =>
        {
            seatBuilder.Property(s => s.Number)
                .HasColumnName("Number")
                .IsRequired();

            seatBuilder.Property(s => s.Row)
                .HasColumnName("Row")
                .HasMaxLength(1).IsFixedLength()
                .IsRequired().IsUnicode(false);

            seatBuilder.WithOwner();
        });
    }
}