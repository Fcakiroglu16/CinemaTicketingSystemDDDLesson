#region

using CinemaTicketingSystem.Domain.BoundedContexts.Accounts;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.BoundedContexts.Purchases;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.Infrastructure.Persistence.Accounts;
using CinemaTicketingSystem.Infrastructure.Persistence.Interceptors;
using CinemaTicketingSystem.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IDomainEventMediator? domainEventMediator,
    IIntegrationEventBus? integrationEventBus)
    : IdentityDbContext<AppUser, AppRole, Guid>(options)
{
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Movie> Movies { get; set; }


    public DbSet<CinemaHallSnapshot> CinemaHallSchedules { get; set; }

    public DbSet<MovieSnapshot> MovieSchedules { get; set; }

    public DbSet<Schedule> Schedules { get; set; }


    public DbSet<RefreshToken> RefreshTokens { get; set; }


    public DbSet<TicketIssuance> TicketIssuance { get; set; }

    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<SeatHold> SeatHolds { get; set; }


    public DbSet<Purchase> Purchases { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();

        optionsBuilder.AddInterceptors(new DomainEventsInterceptor(integrationEventBus, domainEventMediator));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            foreach (IMutableProperty mutableProperty in entityType.GetProperties())
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