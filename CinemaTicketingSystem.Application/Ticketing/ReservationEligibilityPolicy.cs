#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing;

public class ReservationEligibilityPolicy : IDomainService
{
    private const int ReservationCutoffHours = 6;

    public DomainResult ValidateOwnershipAndValidity(
        List<SeatHold> seatHolds,
        List<SeatPosition> requestedSeats)
    {
        if (seatHolds.Count != requestedSeats.Count) return DomainResult.Failure(ErrorCodes.SeatHoldNotFound);

        foreach (SeatHold seatHold in seatHolds)
        {
            bool match = requestedSeats.Any(seat =>
                seatHold.SeatPosition == new SeatPosition(seat.Row, seat.Number));

            if (!match)
                return DomainResult.Failure(ErrorCodes.SeatHoldNotFound, seatHold.SeatPosition.Row,
                    seatHold.SeatPosition.Number);

            if (!seatHold.CanBeConvertedToReservationOrPurchase())
                return DomainResult.Failure(ErrorCodes.SeatHoldExpired, seatHold.SeatPosition.Row,
                    seatHold.SeatPosition.Number);
        }

        return DomainResult.Success();
    }

    public DomainResult IsReservationTooLate(TimeOnly movieStartTime, DateOnly screeningDate)
    {
        DateTime now = DateTime.UtcNow;
        DateTime screeningDateTime = new DateTime(
            screeningDate.Year,
            screeningDate.Month,
            screeningDate.Day,
            movieStartTime.Hour,
            movieStartTime.Minute,
            movieStartTime.Second);

        TimeSpan timeSpan = screeningDateTime - now;
        TimeSpan cutoff = TimeSpan.FromHours(ReservationCutoffHours);

        if (timeSpan < cutoff)
            return DomainResult.Failure(ErrorCodes.ReservationTooLate);

        return DomainResult.Success();
    }
}