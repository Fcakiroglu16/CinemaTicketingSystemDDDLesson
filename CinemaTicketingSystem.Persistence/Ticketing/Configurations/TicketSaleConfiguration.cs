using CinemaTicketingSystem.Domain.Ticketing.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Ticketing.Configurations
{
    internal class TicketSaleConfiguration : IEntityTypeConfiguration<TicketSale>
    {
        public void Configure(EntityTypeBuilder<TicketSale> builder)
        {
            // Basic entity configuration
            builder.ToTable("TicketSales", "Ticketing");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();



            // Configure scalar properties
            builder.Property(x => x.TicketCode).IsFixedLength().HasMaxLength(6)
                .IsRequired();

            builder.Property(x => x.IsUsed);
            builder.Property(x => x.UsedAt);

            // Configure SeatNumber as owned type
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

            // Configure Price as owned type
            builder.OwnsOne(x => x.Price, priceBuilder =>
            {
                priceBuilder.Property(p => p.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(9, 2)
                    .IsRequired();

                priceBuilder.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });
        }
    }
}
