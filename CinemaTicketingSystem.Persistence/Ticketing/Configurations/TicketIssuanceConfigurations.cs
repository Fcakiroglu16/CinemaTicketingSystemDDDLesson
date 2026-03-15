#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Ticketing.Configurations;

internal class TicketIssuanceConfigurations : IEntityTypeConfiguration<TicketIssuance>
{
    public void Configure(EntityTypeBuilder<TicketIssuance> builder)
    {
        // Configure primary key and table
        builder.ToTable("TicketIssuance", "Ticketing");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        // Configure properties
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.ScheduledMovieShowId).IsRequired();


        builder.Property(x => x.CustomerId)
            .HasConversion(
                customerId => customerId.Value,
                value => new CustomerId(value)
            );


        builder.OwnsOne(x => x.TotalPrice, priceBuilder =>
        {
            priceBuilder.Property(p => p.Amount)
                .HasColumnName("Amount")
                //.HasPrecision(9, 2)
                .IsRequired();

            priceBuilder.Property(p => p.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        //builder.Metadata.FindNavigation(nameof(MovieTicket.TicketSales))!.SetPropertyAccessMode(
        //    PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(TicketIssuance.TicketList))!.SetField("_ticketList");


        builder.HasMany(x => x.TicketList).WithOne(y => y.TicketIssuance);
    }
}