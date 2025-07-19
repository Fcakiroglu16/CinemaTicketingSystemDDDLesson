using CinemaTicketingSystem.Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Scheduling;

public class MovieScheduleConfiguration : IEntityTypeConfiguration<MovieSchedule>
{
    public void Configure(EntityTypeBuilder<MovieSchedule> builder)
    {
        // Table configuration
        builder.ToTable("MovieSchedules", "scheduling");

        // Primary key
        builder.HasKey(ms => ms.Id);

        // Properties
        builder.Property(ms => ms.Id)
            .ValueGeneratedNever();

        builder.Property(ms => ms.MovieId)
            .IsRequired();

        // Owned type for Duration (Value Object)
        builder.OwnsOne(m => m.Duration, duration =>
        {
            duration.Property(d => d.Minutes)
                .IsRequired()
                .HasColumnName("DurationMinutes");

            // Computed columns for convenience
            duration.Ignore(d => d.Hours);
            duration.Ignore(d => d.RemainingMinutes);
        });

        builder.Property(ms => ms.SupportedTechnology);

        // Relationships
        builder.HasMany(ms => ms.ShowTimes).WithOne(st => st.MovieSchedule);

        builder.HasMany(x => x.CinemaHallSchedules).WithMany(x => x.MovieSchedules);

        //  builder.Metadata.FindNavigation(nameof(MovieSchedule.CinemaHallSchedules))!.SetField("cinemaHallSchedules");


    }
}