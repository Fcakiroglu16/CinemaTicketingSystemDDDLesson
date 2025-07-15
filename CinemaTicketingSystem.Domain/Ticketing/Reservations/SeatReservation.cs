using CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;
using CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;
using CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations;

public class SeatReservation : AggregateRoot<Guid>
{
    private const int MaxSeatsPerReservation = 10;
    private const int ReservationExpiryMinutes = 30;
    public Guid CustomerId { get; private set; }
    public Guid MovieSessionId { get; private set; }
    public DateTime ReservationTime { get; private set; }
    public DateTime ExpirationTime { get; private set; }
    public ReservationStatus Status { get; private set; }

    private List<ReservedSeat> reservedSeats { get; } = [];

    public IReadOnlyCollection<ReservedSeat> ReservedSeats => reservedSeats.AsReadOnly();

    public void Create(Guid movieSessionId, Guid customerId)
    {
        Id = Guid.CreateVersion7();
        MovieSessionId = movieSessionId;
        CustomerId = customerId;
        ReservationTime = DateTime.UtcNow;
        ExpirationTime = ReservationTime.AddMinutes(ReservationExpiryMinutes);
        Status = ReservationStatus.Pending;

        AddDomainEvent(new ReservationCreatedEvent(Id, CustomerId, MovieSessionId, ReservationTime));
    }

    public void AddSeat(ReservedSeat seat)
    {
        if (reservedSeats.Count >= MaxSeatsPerReservation)
            throw new MaxSeatLimitExceededException(MaxSeatsPerReservation);

        if (reservedSeats.Any(s => s.SeatNumber == seat.SeatNumber))
            throw new DuplicateReservedSeatException(seat.SeatNumber);

        if (Status != ReservationStatus.Pending)
            throw new InvalidReservationStateException(Status, "add seats");

        reservedSeats.Add(seat);
        AddDomainEvent(new SeatReservedEvent(Id, seat.SeatNumber, CustomerId));
    }

    public void RemoveSeat(SeatNumber seatNumber)
    {
        var seat = reservedSeats.FirstOrDefault(s => s.SeatNumber == seatNumber);
        if (seat == null)
            throw new ReservedSeatNotFoundException(seatNumber);

        if (Status != ReservationStatus.Pending)
            throw new InvalidReservationStateException(Status, "remove seats");

        reservedSeats.Remove(seat);
        AddDomainEvent(new SeatReservationReleasedEvent(Id, seatNumber));
    }

    public void AddSeats(IEnumerable<ReservedSeat> seats)
    {
        foreach (var seat in seats) AddSeat(seat);
    }

    public bool HasSeat(SeatNumber seatNumber)
    {
        return reservedSeats.Any(s => s.SeatNumber == seatNumber);
    }

    public void Confirm()
    {
        if (Status != ReservationStatus.Pending)
            throw new InvalidReservationStateException(Status, "confirm");

        if (!reservedSeats.Any())
            throw new EmptyReservationException();

        if (DateTime.UtcNow > ExpirationTime)
            throw new ReservationExpiredException(ExpirationTime);

        Status = ReservationStatus.Confirmed;
        AddDomainEvent(new ReservationConfirmedEvent(Id, CustomerId, MovieSessionId));
    }

    public void Cancel()
    {
        if (Status == ReservationStatus.Canceled)
            return;

        if (Status == ReservationStatus.Expired)
            throw new InvalidReservationStateException(Status, "cancel");

        Status = ReservationStatus.Canceled;
        AddDomainEvent(new ReservationCanceledEvent(Id, CustomerId, MovieSessionId));
    }

    public void Expire()
    {
        if (Status != ReservationStatus.Pending)
            return;

        Status = ReservationStatus.Expired;
        AddDomainEvent(new ReservationExpiredEvent(Id, CustomerId, MovieSessionId));
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpirationTime && Status == ReservationStatus.Pending;
    }
}