using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;

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


        Id = cinemaHallId;
        SeatCount = seatCount;
        SupportedTechnologies = supportedTechnologies;
    }


    public ScreeningTechnology SupportedTechnologies { get; private set; } = ScreeningTechnology.Standard;


    public short SeatCount { get; set; }
}