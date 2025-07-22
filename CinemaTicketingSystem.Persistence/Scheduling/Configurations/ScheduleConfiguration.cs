using CinemaTicketingSystem.Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Scheduling.Configurations;

internal class ScheduleConfiguration :
    IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasKey(ms => ms.Id);
        builder.Property(ms => ms.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.MovieId).IsRequired();
        builder.Property(x => x.HallId).IsRequired();
        // Configure ShowTime as owned type
        builder.OwnsOne(x => x.ShowTime, showTime =>
        {
            showTime.Property(st => st.StartTime)
                .IsRequired()
                .HasColumnName("ShowTime_StartTime")
                .HasColumnType("time");

            showTime.Property(st => st.EndTime)
                .IsRequired()
                .HasColumnName("ShowTime_EndTime")
                .HasColumnType("time");
        });
    }
}