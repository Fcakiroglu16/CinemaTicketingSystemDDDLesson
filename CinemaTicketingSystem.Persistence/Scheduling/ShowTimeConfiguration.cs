using CinemaTicketingSystem.Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaTicketingSystem.Persistence.Scheduling;

public class ShowTimeConfiguration : IEntityTypeConfiguration<ShowTime>
{
    public void Configure(EntityTypeBuilder<ShowTime> builder)
    {
        // Table configuration
        builder.ToTable("ShowTimes", "scheduling");

        // Primary key
        builder.HasKey(st => st.Id);

        // Properties
        builder.Property(st => st.Id)
            .ValueGeneratedNever();

        builder.Property(st => st.StartTime)
            .IsRequired()
            .HasColumnType("time");

        builder.Property(st => st.EndTime)
            .IsRequired()
            .HasColumnType("time");

    }
}