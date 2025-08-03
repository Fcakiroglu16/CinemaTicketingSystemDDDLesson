using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.Persistence.Accounts;
using CinemaTicketingSystem.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CinemaTicketingSystem.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher)
    : IdentityDbContext<AppUser, AppRole, Guid>(options)
{
    public DbSet<Purchase> MovieTickets { get; set; }

    public DbSet<Reservation> SeatReservations { get; set; }


    public DbSet<CinemaHallSnapshot> CinemaHallSchedules { get; set; }

    public DbSet<MovieSnapshot> MovieSchedules { get; set; }

    public DbSet<Schedule> Schedules { get; set; }


    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Movie> Movies { get; set; }


    public DbSet<Purchase> TicketPurchases { get; set; }
    public DbSet<Ticket> Tickets { get; set; }


    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();

        optionsBuilder.AddInterceptors(new DomainEventsInterceptor(publisher));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            foreach (var mutableProperty in entityType.GetProperties())
            {
                if (!ReferenceEquals(mutableProperty.ClrType, typeof(decimal))) continue;
                mutableProperty.SetPrecision(9);
                mutableProperty.SetScale(2);
            }

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<AppUser>().ToTable("Users", "accounts");
        modelBuilder.Entity<AppRole>().ToTable("Roles", "accounts");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", "accounts");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", "accounts");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", "accounts");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", "accounts");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", "accounts");
    }
}