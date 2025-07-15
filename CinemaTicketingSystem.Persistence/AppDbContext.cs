using CinemaTicketingSystem.Domain.Ticketing.Reservations;
using CinemaTicketingSystem.Domain.Ticketing.Tickets;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CinemaTicketingSystem.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    public DbSet<MovieTicket> MovieTickets { get; set; }

    public DbSet<SeatReservation> SeatReservations { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            foreach (var mutableProperty in entityType.GetProperties())
            {
                if (!ReferenceEquals(mutableProperty.ClrType, typeof(decimal))) continue;
                mutableProperty.SetPrecision(9);
                mutableProperty.SetScale(2);
            }


        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}