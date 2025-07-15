using CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations;

public class ReservedSeat : Entity<Guid>
{
    internal ReservedSeat(SeatNumber seatNumber)
    {
        Id = Guid.CreateVersion7();
        SeatNumber = seatNumber;
    }

    public SeatNumber SeatNumber { get; }

    public SeatReservation SeatReservation { get; set; } = null!;
    public string GetSeatInfo()
    {
        return $"Seat: {SeatNumber}";
    }
}