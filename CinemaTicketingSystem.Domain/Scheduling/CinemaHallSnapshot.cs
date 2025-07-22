using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Scheduling;

public class CinemaHallSnapshot : AggregateRoot<Guid>
{
    protected CinemaHallSnapshot()
    {
    }


    public CinemaHallSnapshot(Guid cinemaHallId, short seatCount, ScreeningTechnology supportedTechnologies)
    {
        if (cinemaHallId == Guid.Empty)
            throw new ArgumentException("Cinema hall ID cannot be empty", nameof(cinemaHallId));
        if (seatCount <= 0) throw new ArgumentOutOfRangeException(nameof(seatCount), "Seat count must be positive");

        Id = Guid.CreateVersion7();
        CinemaHallId = cinemaHallId;
        SeatCount = seatCount;
        SupportedTechnologies = supportedTechnologies;
    }

    public Guid CinemaHallId { get; set; }

    public ScreeningTechnology SupportedTechnologies { get; private set; } = ScreeningTechnology.Standard;


    public short SeatCount { get; set; }
}