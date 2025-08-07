using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

namespace CinemaTicketingSystem.Application.Ticketing
{
    public class ReservationEligibilityPolicy : IDomainService
    {
        public DomainResult ValidateOwnershipAndValidity(
            List<SeatHold> seatHolds,
            List<SeatPosition> requestedSeats)
        {
            if (seatHolds.Count != requestedSeats.Count)
            {
                return DomainResult.Failure(ErrorCodes.SeatHoldNotFound);
            }

            foreach (var seatHold in seatHolds)
            {
                var match = requestedSeats.Any(seat =>
                    seatHold.SeatPosition == new SeatPosition(seat.Row, seat.Number));

                if (!match)
                {
                    return DomainResult.Failure(ErrorCodes.SeatHoldNotFound,
                        [seatHold.SeatPosition.Row, seatHold.SeatPosition.Number]);
                }

                if (!seatHold.CanBeConvertedToReservationOrPurchase())
                {
                    return DomainResult.Failure(ErrorCodes.SeatHoldExpired,
                        [seatHold.SeatPosition.Row, seatHold.SeatPosition.Number]);
                }
            }

            return DomainResult.Success();
        }

        public DomainResult IsReservationTooLate(TimeOnly movieStartTime, DateTime now)
        {
            var todayStartTime = now.Date + movieStartTime.ToTimeSpan();
            var result = (todayStartTime - now).TotalHours < 2;

            return result ? DomainResult.Failure(ErrorCodes.ReservationTooLate) : DomainResult.Success();
        }
    }
}