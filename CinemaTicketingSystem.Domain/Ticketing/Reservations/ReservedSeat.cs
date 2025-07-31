using CinemaTicketingSystem.Domain.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations;

public class ReservedSeat : Entity<Guid>
{
    internal ReservedSeat(SeatNumber seatNumber)
    {
        Id = Guid.CreateVersion7();
        SeatNumber = seatNumber;
    }

    private ReservedSeat()
    {
    }

    public SeatNumber SeatNumber { get; } = null!;

    public virtual SeatReservation SeatReservation { get; set; } = null!;

    public string GetSeatInfo()
    {
        return $"Seat: {SeatNumber}";
    }
}