using CinemaTicketingSystem.Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Scheduling;

public class CinemaHallScheduleConfiguration : IEntityTypeConfiguration<CinemaHallSchedule>
{
    public void Configure(EntityTypeBuilder<CinemaHallSchedule> builder)
    {
        // Table configuration
        builder.ToTable("CinemaHallSchedules", "scheduling");

        // Primary key
        builder.HasKey(ms => ms.Id);

        // Properties
        builder.Property(ms => ms.Id)
            .ValueGeneratedNever();


        builder.Property(ms => ms.CinemaHallId).IsRequired();
        builder.Property(ms => ms.SupportedTechnologies);

        // Relationships
        builder.HasMany(x => x.MovieSchedules).WithMany(x => x.CinemaHallSchedules);

        // builder.Metadata.FindNavigation(nameof(CinemaHallSchedule.MovieSchedules))!.SetField("movieSchedules");
    }
}