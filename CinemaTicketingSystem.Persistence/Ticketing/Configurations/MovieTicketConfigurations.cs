using CinemaTicketingSystem.Domain.Ticketing.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Ticketing.Configurations
{
    internal class MovieTicketConfigurations : IEntityTypeConfiguration<MovieTicket>
    {
        public void Configure(EntityTypeBuilder<MovieTicket> builder)
        {
            // Configure primary key and table
            builder.ToTable("MovieTickets", "Ticketing");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            // Configure properties
            builder.Property(x => x.CustomerId);
            builder.Property(x => x.MovieSessionId);
            builder.Property(x => x.IsDiscountApplied);




            builder.Metadata.FindNavigation(nameof(MovieTicket.TicketSales))!.SetPropertyAccessMode(
                PropertyAccessMode.Field);

            //builder.Metadata.FindNavigation(nameof(MovieTicket.PurchasedTickets))!.SetField("_purchasedTickets");



            //builder.HasMany(x => x.TicketSales).WithOne(y => y.MovieTicket);




        }
    }
}
