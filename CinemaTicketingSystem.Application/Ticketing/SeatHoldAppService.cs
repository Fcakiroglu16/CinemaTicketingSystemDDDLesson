using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.ValueObjects;
using System.Net;

namespace CinemaTicketingSystem.Application.Ticketing;

public class SeatHoldAppService(AppDependencyService appDependencyService, ISeatHoldRepository seatHoldRepository)
    : IScopedDependency, ISeatHoldAppService
{
    public async Task<AppResult> CreateSeatHoldAsync(CreateSeatHoldRequest request)
    {
        var customerId = appDependencyService.UserContext.UserId;


        //TODO: redis lock eklenebilir
        var seatHold = await seatHoldRepository.WhereAsync(x => x.ScheduledMovieShowId == request.ScheduledMovieShowId);


        foreach (var seat in request.SeatPosition.Where(seat =>
                     seatHold.Any(x => x.SeatPosition == new SeatPosition(seat.Row, seat.Number))))
            return appDependencyService.Error(ErrorCodes.SeatAlreadyHeld, [seat.Row, seat.Number],
                HttpStatusCode.BadRequest);


        foreach (var newSeatHold in request.SeatPosition.Select(seat =>
                     new SeatHold(request.ScheduledMovieShowId, customerId, new SeatPosition(seat.Row, seat.Number))))
            await seatHoldRepository.AddAsync(newSeatHold);


        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }
}