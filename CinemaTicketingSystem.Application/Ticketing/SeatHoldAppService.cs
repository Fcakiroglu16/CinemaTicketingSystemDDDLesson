#region

using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing;

public class SeatHoldAppService(AppDependencyService appDependencyService, ISeatHoldRepository seatHoldRepository)
    : IScopedDependency, ISeatHoldAppService
{
    public async Task<AppResult> CreateAsync(CreateSeatHoldRequest request)
    {
        var customerId = appDependencyService.UserContext.UserId;


        //TODO: redis lock can add here for concurrency handling
        var seatHold =
            (await seatHoldRepository.WhereAsync(x =>
                x.ScheduledMovieShowId == request.ScheduledMovieShowId && x.ScreeningDate == request.ScreeningDate &&
                x.Status == HoldStatus.Hold && x.ExpiresAt < DateTime.UtcNow)).ToList();


        var hasSeatPositionList = request.SeatPositions.Where(seat =>
            seatHold.Any(x => x.SeatPosition.Equals(new SeatPosition(seat.Row, seat.Number)))).ToList();


        if (hasSeatPositionList.Any())
        {
            var seat = hasSeatPositionList.First();
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatAlreadyHeld, [seat.Row, seat.Number]);
        }


        var customerSeatHolds = await seatHoldRepository.WhereAsync(x =>
            x.CustomerId == customerId && x.ScreeningDate.Equals(request.ScreeningDate) &&
            x.ScheduledMovieShowId == request.ScheduledMovieShowId);


        //idempotency check

        var newSeats = request.SeatPositions.ToList();


        foreach (var seat in request.SeatPositions.Where(seat =>
                     customerSeatHolds.Any(x => x.SeatPosition.Equals(new SeatPosition(seat.Row, seat.Number)))))
            newSeats.Remove(seat);


        foreach (var newSeatHold in newSeats.Select(seat =>
                     new SeatHold(request.ScheduledMovieShowId, customerId, new SeatPosition(seat.Row, seat.Number),
                         request.ScreeningDate)))
            await seatHoldRepository.AddAsync(newSeatHold);


        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }


    public async Task<AppResult> ConfirmAsync(ConfirmSeatHoldRequest request)
    {
        var customerId = appDependencyService.UserContext.UserId;

        var seatHolds = (await seatHoldRepository.WhereAsync(x =>
                x.ScheduledMovieShowId == request.ScheduledMovieShowId && x.ScreeningDate == request.ScreeningDate &&
                x.CustomerId == customerId))
            .ToList();

        foreach (var seatHold in seatHolds)
        {
            seatHold.ConfirmHold();
            await seatHoldRepository.UpdateAsync(seatHold);
        }

        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }

    public async Task<AppResult> Cancel()
    {
        var customerId = appDependencyService.UserContext.UserId;

        var seatHolds = (await seatHoldRepository.WhereAsync(x => x.CustomerId == customerId)).ToList();

        await seatHoldRepository.DeleteRangeAsync(seatHolds);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }
}