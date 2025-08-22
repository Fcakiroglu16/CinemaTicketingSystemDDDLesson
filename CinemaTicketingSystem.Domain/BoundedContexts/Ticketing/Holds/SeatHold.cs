#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds.DomainEvents;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;

public class SeatHold : AggregateRoot<Guid>
{
    private const int DefaultHoldDurationMinutes = 5;

    protected SeatHold()
    {
    } // For EF Core

    public SeatHold(Guid scheduledMovieShowId, Guid customerId, SeatPosition seatPosition, DateOnly screeningDate)
    {
        Id = Guid.CreateVersion7();
        ScheduledMovieShowId = scheduledMovieShowId;
        CustomerId = customerId;
        SeatPosition = seatPosition;
        Status = HoldStatus.Active;
        ScreeningDate = screeningDate;

        AddDomainEvent(new SeatHoldStarted(ScheduledMovieShowId, CustomerId, screeningDate, SeatPosition));
    }

    public Guid ScheduledMovieShowId { get; private set; }

    public DateOnly ScreeningDate { get; private set; }
    public Guid CustomerId { get; private set; }
    public SeatPosition SeatPosition { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    public HoldStatus Status { get; private set; }


    public void ConfirmHold()
    {
        if (IsExpired())
            throw new BusinessException(ErrorCodes.SeatHoldExpired);
        Status = HoldStatus.Confirm;
        ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromMinutes(DefaultHoldDurationMinutes));
        AddDomainEvent(new SeatHoldConfirmed(ScheduledMovieShowId, CustomerId, ScreeningDate, SeatPosition));
    }

    public void ExtendHold(TimeSpan additionalTime)
    {
        if (IsExpired())
            throw new BusinessException(ErrorCodes.SeatHoldExpired);

        ExpiresAt = ExpiresAt?.Add(additionalTime);
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpiresAt;
    }

    public bool CanBeConvertedToReservationOrPurchase()
    {
        return !IsExpired();
    }

    public bool IsHold()
    {
        return Status == HoldStatus.Confirm && !IsExpired();
    }
}