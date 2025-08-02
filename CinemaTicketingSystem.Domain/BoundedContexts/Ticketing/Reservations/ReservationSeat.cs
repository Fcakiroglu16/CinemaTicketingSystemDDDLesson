using CinemaTicketingSystem.Domain.ValueObjects;
using CinemaTicketingSystem.SharedKernel.Entities;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;

public class ReservationSeat : Entity<Guid>
{
    public ReservationSeat(SeatPosition seatPosition)
    {
        Id = Guid.CreateVersion7();
        SeatPosition = seatPosition;
    }

    protected ReservationSeat()
    {
    }

    public SeatPosition SeatPosition { get; } = null!;

    public virtual Reservation Reservation { get; set; } = null!;

    public string GetSeatInfo()
    {
        return $"Seat: {SeatPosition}";
    }
}