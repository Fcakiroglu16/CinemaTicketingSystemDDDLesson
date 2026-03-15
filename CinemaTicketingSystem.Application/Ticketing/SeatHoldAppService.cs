#region

using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds.Specifications;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing;

public class SeatHoldAppService(AppDependencyService appDependencyService, ISeatHoldRepository seatHoldRepository)
    : IScopedDependency, ISeatHoldAppService
{
    public async Task<AppResult> CreateAsync(CreateSeatHoldRequest request)
    {
        Guid customerId = appDependencyService.UserContext.UserId;


        //TODO: redis lock can add here for concurrency handling
        List<SeatHold> seatHold =
            await seatHoldRepository.ListAsync(
                new ActiveSeatHoldsByScheduleAndDateSpec(request.ScheduledMovieShowId, request.ScreeningDate));


        List<SeatPositionDto> hasSeatPositionList = request.SeatPositions.Where(seat =>
            seatHold.Any(x => x.SeatPosition.Equals(new SeatPosition(seat.Row, seat.Number)))).ToList();


        if (hasSeatPositionList.Any())
        {
            SeatPositionDto seat = hasSeatPositionList.First();
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatAlreadyHeld, [seat.Row, seat.Number]);
        }


        List<SeatHold> customerSeatHolds = await seatHoldRepository.ListAsync(
            new UserSeatHoldsByScheduleAndDateSpec(customerId, request.ScheduledMovieShowId, request.ScreeningDate));


        //idempotency check

        List<SeatPositionDto> newSeats = request.SeatPositions.ToList();


        foreach (SeatPositionDto? seat in request.SeatPositions.Where(seat =>
                     customerSeatHolds.Any(x => x.SeatPosition.Equals(new SeatPosition(seat.Row, seat.Number)))))
            newSeats.Remove(seat);


        foreach (SeatHold? newSeatHold in newSeats.Select(seat =>
                     new SeatHold(request.ScheduledMovieShowId, customerId, new SeatPosition(seat.Row, seat.Number),
                         request.ScreeningDate)))
            await seatHoldRepository.AddAsync(newSeatHold);


        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }


    public async Task<AppResult> ConfirmAsync(ConfirmSeatHoldRequest request)
    {
        Guid customerId = appDependencyService.UserContext.UserId;


        List<SeatHold> seatHolds = await seatHoldRepository.ListAsync(
            new UserSeatHoldsByScheduleAndDateSpec(customerId, request.ScheduledMovieShowId, request.ScreeningDate));


        foreach (SeatHold? seatHold in seatHolds)
        {
            seatHold.ConfirmHold();
            await seatHoldRepository.UpdateAsync(seatHold);
        }

        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }

    public async Task<AppResult> Cancel()
    {
        Guid customerId = appDependencyService.UserContext.UserId;

        List<SeatHold> seatHolds = await seatHoldRepository.ListAsync(
            new SeatHoldsByCustomerSpec(customerId));

        await seatHoldRepository.DeleteRangeAsync(seatHolds);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }
}