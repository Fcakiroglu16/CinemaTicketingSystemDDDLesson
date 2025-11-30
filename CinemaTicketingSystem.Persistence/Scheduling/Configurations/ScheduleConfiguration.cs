#region

using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Scheduling.Configurations;

internal class ScheduleConfiguration :
    IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules", "scheduling");
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

        // Configure Price as owned type
        builder.OwnsOne(x => x.TicketPrice, priceBuilder =>
        {
            priceBuilder.Property(p => p.Amount)
                .HasColumnName("Amount")
                .HasPrecision(9, 2)
                .IsRequired();

            priceBuilder.Property(p => p.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });
    }
}