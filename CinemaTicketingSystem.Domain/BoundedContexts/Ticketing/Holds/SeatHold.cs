#region

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

    public SeatHold(Guid scheduledMovieShowId, Guid customerId, SeatPosition seatPosition)
    {
        Id = Guid.CreateVersion7();
        ScheduledMovieShowId = scheduledMovieShowId;
        CustomerId = customerId;
        SeatPosition = seatPosition;
        ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromMinutes(DefaultHoldDurationMinutes));
    }

    public Guid ScheduledMovieShowId { get; private set; }
    public Guid CustomerId { get; private set; }
    public SeatPosition SeatPosition { get; private set; }
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