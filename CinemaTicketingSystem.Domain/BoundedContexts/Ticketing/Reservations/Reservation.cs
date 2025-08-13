#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations.DomainEvents;
using CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;

public class Reservation : AggregateRoot<Guid>
{
    private const int MaxSeatCountPerReservation = 10;
    private const int ReservationCutoffHours = -4;

    private readonly List<ReservationSeat> _reservationSeatList = [];


    protected Reservation()
    {
    }


    public Reservation(Guid scheduleId, Guid customerId, DateOnly screeningDate, TimeOnly MovieStartTime)
    {
        Id = Guid.CreateVersion7();
        ScheduledMovieShowId = scheduleId;
        CustomerId = customerId;
        ReservationTime = DateTime.UtcNow;

        AddDomainEvent(new ReservationCreatedEvent(Id, CustomerId, scheduleId, ReservationTime));
        ScreeningDate = screeningDate;

        var movieStartDateTime = screeningDate.ToDateTime(MovieStartTime);
        ExpirationTime = movieStartDateTime.AddHours(ReservationCutoffHours);
    }

    public Guid CustomerId { get; }
    public Guid ScheduledMovieShowId { get; }
    public DateTime ReservationTime { get; }
    public DateTime ExpirationTime { get; }

    public DateOnly ScreeningDate { get; private set; }
    public ReservationStatus Status { get; private set; }

    public virtual IReadOnlyCollection<ReservationSeat> ReservationSeatList => _reservationSeatList.AsReadOnly();

    public void AddSeat(ReservationSeat reservationSeat)
    {
        if (_reservationSeatList.Count >= MaxSeatCountPerReservation)
            throw new BusinessException(ErrorCodes.MaxSeatsPerReservation).AddData(MaxSeatCountPerReservation);

        if (_reservationSeatList.Any(s => s.SeatPosition == reservationSeat.SeatPosition))
            throw new BusinessException(ErrorCodes.SeatAlreadyReserved)
                .AddData(reservationSeat.SeatPosition.Row)
                .AddData(reservationSeat.SeatPosition.Number);

        _reservationSeatList.Add(reservationSeat);
        AddDomainEvent(new SeatReservedEvent(Id, ScheduledMovieShowId, CustomerId, reservationSeat.SeatPosition));
    }

    public void RemoveSeat(SeatPosition seatPosition)
    {
        var seat = _reservationSeatList.FirstOrDefault(s => s.SeatPosition == seatPosition);
        if (seat == null)
            throw new BusinessException(ErrorCodes.ReservedSeatNotFound)
                .AddData(seatPosition.Row)
                .AddData(seatPosition.Number);

        _reservationSeatList.Remove(seat);
        AddDomainEvent(new SeatReservationReleasedEvent(Id, seatPosition));
    }

    public void AddSeats(IEnumerable<ReservationSeat> seats)
    {
        foreach (var seat in seats) AddSeat(seat);
    }

    public bool HasSeat(SeatPosition seatPosition)
    {
        return _reservationSeatList.Any(s => s.SeatPosition == seatPosition);
    }

    public void Confirm()
    {
        if (_reservationSeatList.Count == 0)
            throw new BusinessException(ErrorCodes.NoSeatsReserved);

        Status = ReservationStatus.Confirmed;
        AddDomainEvent(new ReservationConfirmedEvent(Id, CustomerId, ScheduledMovieShowId));
    }

    public void Cancel()
    {
        if (Status == ReservationStatus.Canceled)
            return;

        if (Status == ReservationStatus.Expired)
            throw new BusinessException(ErrorCodes.CannotCancelExpiredReservation);

        Status = ReservationStatus.Canceled;
        AddDomainEvent(new ReservationCanceledEvent(Id, CustomerId, ScheduledMovieShowId));
    }

    public void Expire()
    {
        Status = ReservationStatus.Expired;
        AddDomainEvent(new ReservationExpiredEvent(Id, CustomerId, ScheduledMovieShowId));
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpirationTime;
    }
}