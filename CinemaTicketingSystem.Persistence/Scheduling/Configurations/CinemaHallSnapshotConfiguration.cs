#region

using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Scheduling.Configurations;

public class CinemaHallSnapshotConfiguration : IEntityTypeConfiguration<CinemaHallSnapshot>
{
    public void Configure(EntityTypeBuilder<CinemaHallSnapshot> builder)
    {
        // Table configuration
        builder.ToTable("CinemaHallSnapshots", "scheduling");

        // Primary key
        builder.HasKey(ms => ms.Id);

        // Properties
        builder.Property(ms => ms.Id)
            .ValueGeneratedNever();


        builder.Property(ms => ms.SupportedTechnologies);
    }
}