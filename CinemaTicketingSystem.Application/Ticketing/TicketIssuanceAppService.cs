#region

using CinemaTicketingSystem.Application.Catalog.Services;
using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Application.Schedules.Services;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds.Specifications;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.Specifications;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;


#endregion

namespace CinemaTicketingSystem.Application.Ticketing;

public class TicketIssuanceAppService(
    AppDependencyService appDependencyService,
    ITicketIssuanceRepository ticketIssuanceRepository,
    ICatalogQueryService catalogQueryService,
    IScheduleQueryService iScheduleQueryService,
    ISeatHoldRepository seatHoldRepository,
    IReservationRepository reservationRepository) : IScopedDependency, ITicketPurchaseAppService
{
    // Pseudocode:
    // - Fetch confirmed ticket seat positions for the schedule/date.
    // - Fetch confirmed seat-hold positions for the schedule/date.
    // - Merge both lists with unique items only (by Row and Number).
    // - Use the merged unique list to check if user's held seats conflict.
    // - Proceed with ticket issuance if no conflicts.
    //
    // Implementation notes:
    // - Use Concat + DistinctBy((Row, Number)) to union uniquely.
    // - Compare seats by Row and Number when checking conflicts.

    public async Task<AppResult<CreateTicketIssuanceResponse>> Create(CreateTicketIssuanceRequest request)
    {
        Guid userId = appDependencyService.UserContext.UserId;

        AppResult<GetScheduleInfoResponse> scheduleInfo = await iScheduleQueryService.GetScheduleInfo(request.ScheduledMovieShowId);

        if (scheduleInfo.IsFail)
            return AppResult<CreateTicketIssuanceResponse>.Error(scheduleInfo.ProblemDetails!);


        AppResult<GetCatalogInfoResponse> catalogInfo =
            await catalogQueryService.GetCinemaInfo(scheduleInfo.Data!.CinemaHallId, scheduleInfo.Data.MovieId);

        if (catalogInfo.IsFail) return AppResult<CreateTicketIssuanceResponse>.Error(catalogInfo.ProblemDetails!);


        List<SeatHold> userSeatHoldList = await seatHoldRepository.ListAsync(
            new UserSeatHoldsByScheduleAndDateSpec(userId, request.ScheduledMovieShowId, request.ScreeningDate));


        if (!userSeatHoldList.Any())
            return appDependencyService.LocalizeError.Error<CreateTicketIssuanceResponse>(ErrorCodes.NoSeatHoldFound);


        if (userSeatHoldList.Any(seatHold => seatHold.IsExpired()))
            return appDependencyService.LocalizeError.Error<CreateTicketIssuanceResponse>(
                ErrorCodes.SeatHoldExpired);


        // Fetch confirmed seats from tickets
        List<SeatPosition> confirmedTicketSeatPositions =
            (await ticketIssuanceRepository.ListAsync(
                new ConfirmedTicketIssuanceByScheduleAndDateSpec(request.ScheduledMovieShowId, request.ScreeningDate)))
            .SelectMany(x => x.TicketList)
            .Select(x => x.SeatPosition)
            .ToList();

        // Fetch confirmed seats from holds
        List<SeatPosition> confirmedSeatHoldSeatPositions =
            (await seatHoldRepository.ListAsync(
                new ActiveSeatHoldsByScheduleAndDateSpec(request.ScheduledMovieShowId, request.ScreeningDate)))
            .Where(x => x.CustomerId != userId)
            .Select(x => x.SeatPosition)
            .ToList();


        // Merge uniquely by seat coordinates
        List<SeatPosition> occupiedSeatPositions = confirmedTicketSeatPositions
            .Concat(confirmedSeatHoldSeatPositions)
            .DistinctBy(sp => (sp.Row, sp.Number))
            .ToList();


        foreach (SeatHold? seat in userSeatHoldList)
        {
            bool seatTaken = occupiedSeatPositions.Any(x =>
                x.Row == seat.SeatPosition.Row && x.Number == seat.SeatPosition.Number);
            if (seatTaken)
                return appDependencyService.LocalizeError.Error<CreateTicketIssuanceResponse>(ErrorCodes.DuplicateSeat,
                    [seat.SeatPosition.Row, seat.SeatPosition.Number]);
        }

        TicketIssuance newTicketIssuance =
            new TicketIssuance(request.ScheduledMovieShowId, userId, request.ScreeningDate);

        foreach (SeatHold? seat in userSeatHoldList)
            newTicketIssuance.AddTicket(seat.SeatPosition, scheduleInfo.Data.TicketPrice);

        await ticketIssuanceRepository.AddAsync(newTicketIssuance);

        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult<CreateTicketIssuanceResponse>.SuccessAsCreated(
            new CreateTicketIssuanceResponse(newTicketIssuance.Id), string.Empty);
    }


    public async Task<AppResult> CreateFromReservation(Guid reservationId)
    {
        Reservation? reservation = await reservationRepository.GetByIdAsync(reservationId);


        if (reservation!.IsExpired()) return appDependencyService.LocalizeError.Error(ErrorCodes.ReservationExpired);

        TicketIssuance purchase = new TicketIssuance(reservation.ScheduledMovieShowId, reservation.CustomerId,
            reservation.ScreeningDate);


        AppResult<GetScheduleInfoResponse> scheduleInfo = await iScheduleQueryService.GetScheduleInfo(reservation.ScheduledMovieShowId);

        if (scheduleInfo.IsFail) return scheduleInfo;

        foreach (ReservationSeat seat in reservation.ReservationSeatList)
            purchase.AddTicket(seat.SeatPosition, scheduleInfo.Data!.TicketPrice);


        await ticketIssuanceRepository.AddAsync(purchase);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }
}