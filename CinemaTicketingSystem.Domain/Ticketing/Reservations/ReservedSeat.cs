using CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations;

public class ReservedSeat : Entity<Guid>
{
    internal ReservedSeat(SeatNumber seatNumber)
    {
        Id = Guid.CreateVersion7();
        SeatNumber = seatNumber;
        ReservedAt = DateTime.UtcNow;
    }

    public SeatNumber SeatNumber { get; }
    public DateTime ReservedAt { get; }

    public string GetSeatInfo()
    {
        return $"Seat: {SeatNumber}, Reserved at: {ReservedAt}";
    }
}