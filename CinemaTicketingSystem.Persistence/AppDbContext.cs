using CinemaTicketingSystem.Domain.Ticketing.Reservations;
using CinemaTicketingSystem.Domain.Ticketing.Tickets;
using CinemaTicketingSystem.Persistence.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CinemaTicketingSystem.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, AppRole, Guid>(options)
{

    public DbSet<MovieTicket> MovieTickets { get; set; }

    public DbSet<SeatReservation> SeatReservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseLazyLoadingProxies();
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