using CinemaTicketingSystem.Domain.Ticketing.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Ticketing.Configurations;

internal class MovieTicketConfigurations : IEntityTypeConfiguration<TicketPurchase>
{
    public void Configure(EntityTypeBuilder<TicketPurchase> builder)
    {
        // Configure primary key and table
        builder.ToTable("MovieTickets", "Ticketing");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        // Configure properties
        builder.Property(x => x.CustomerId);
        builder.Property(x => x.ScheduleId);
        builder.Property(x => x.IsDiscountApplied);


        //builder.Metadata.FindNavigation(nameof(MovieTicket.TicketSales))!.SetPropertyAccessMode(
        //    PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(TicketPurchase.TicketList))!.SetField("ticketSales");


        builder.HasMany(x => x.TicketList).WithOne(y => y.TicketPurchase);
    }
}