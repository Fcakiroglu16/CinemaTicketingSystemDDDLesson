#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
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


        //builder.Metadata.FindNavigation(nameof(MovieTicket.TicketSales))!.SetPropertyAccessMode(
        //    PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(TicketIssuance.TicketList))!.SetField("_ticketList");


        builder.HasMany(x => x.TicketList).WithOne(y => y.TicketIssuance);
    }
}