using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Core.Exceptions;
using CinemaTicketingSystem.Domain.ValueObjects;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;

public sealed class SeatHold : AggregateRoot<Guid>
{
    protected SeatHold()
    {
    } // For EF Core

    public SeatHold(Guid scheduledMovieShowId, Guid customerId, SeatPosition seatPosition)
    {
        Id = Guid.CreateVersion7();
        ScheduledMovieShowId = scheduledMovieShowId;
        CustomerId = customerId;
        SeatPosition = seatPosition;
        ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromMinutes(5));
    }

    public Guid ScheduledMovieShowId { get; private set; }
    public Guid CustomerId { get; private set; }
    public SeatPosition SeatPosition { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }

    public void ExtendHold(TimeSpan additionalTime)
    {
        if (IsExpired())
            throw new BusinessException(ErrorCodes.SeatHoldExpired);

        ExpiresAt = ExpiresAt.Add(additionalTime);
    }

    private bool IsExpired()
    {
        return DateTime.UtcNow > ExpiresAt;
    }

    public bool CanBeConvertedToReservationOrPurchase()
    {
        return !IsExpired();
    }
}