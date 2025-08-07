#region

using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;
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
    public async Task<AppResult> CreateSeatHoldAsync(CreateSeatHoldRequest request)
    {
        var customerId = appDependencyService.UserContext.UserId;


        //TODO: redis lock eklenebilir
        var seatHold =
            (await seatHoldRepository.WhereAsync(x => x.ScheduledMovieShowId == request.ScheduledMovieShowId)).ToList();


        foreach (var seat in request.SeatPosition.Where(seat =>
                     seatHold.Any(x => x.SeatPosition == new SeatPosition(seat.Row, seat.Number))))
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatAlreadyHeld, [seat.Row, seat.Number]);


        foreach (var newSeatHold in request.SeatPosition.Select(seat =>
                     new SeatHold(request.ScheduledMovieShowId, customerId, new SeatPosition(seat.Row, seat.Number))))
            await seatHoldRepository.AddAsync(newSeatHold);


        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }


    public async Task<AppResult> CancelSeatHold()
    {
        var customerId = appDependencyService.UserContext.UserId;

        var seatHolds = (await seatHoldRepository.WhereAsync(x => x.CustomerId == customerId)).ToList();

        await seatHoldRepository.DeleteRangeAsync(seatHolds);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }
}