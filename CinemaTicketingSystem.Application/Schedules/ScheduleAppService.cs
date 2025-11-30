#region

using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Contracts.Schedule;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.Repositories;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using System.Net;

#endregion

namespace CinemaTicketingSystem.Application.Schedules;

public class ScheduleAppService(
    IGenericRepository<CinemaHallSnapshot> CinemaHallSnapshotRepository,
    IGenericRepository<MovieSnapshot> movieShotRepository,
    MovieHallCompatibilityService movieHallCompatibilityService,
    IScheduleRepository scheduleRepository,
    AppDependencyService appDependencyService) : IScopedDependency, IScheduleAppService
{
    public async Task<AppResult> AddMovieToHall(Guid hallId, AddMovieToHallRequest request)
    {
        MovieSnapshot? movie = await movieShotRepository.GetByIdAsync(request.MovieId);

        if (movie is null) return appDependencyService.LocalizeError.Error(ErrorCodes.MovieNotFound);

        CinemaHallSnapshot? hallSchedule = await CinemaHallSnapshotRepository.GetByIdAsync(hallId);

        if (hallSchedule is null) return appDependencyService.LocalizeError.Error(ErrorCodes.CinemaHallNotFound);

        DomainResult compatibilityResult = movieHallCompatibilityService.IsCompatible(movie, hallSchedule);

        if (!compatibilityResult.IsSuccess)
            return appDependencyService.LocalizeError.Error(compatibilityResult.Error!, compatibilityResult.ErrorData);

        ShowTime showTime;

        if (request.EndTime.HasValue)
        {
            bool result = movie.IsValidDuration(request.StartTime, request.EndTime.Value);
            {
                if (!result) return appDependencyService.LocalizeError.Error(ErrorCodes.MovieDurationMismatch);
            }

            showTime = ShowTime.Create(request.StartTime, request.EndTime.Value);
        }
        else
        {
            showTime = ShowTime.Create(request.StartTime, movie.Duration);
        }

        List<Schedule> schedules = (await scheduleRepository.WhereAsync(x => x.HallId == hallId)).ToList();

        if (schedules.Any(x => x.ShowTime.ConflictsWith(showTime)))
        {
            string conflictingShowTimes = string.Join(", ",
                schedules.Where(x => x.ShowTime.ConflictsWith(showTime))
                    .Select(x => x.ShowTime.GetDisplayInfo()));


            return appDependencyService.LocalizeError.Error(ErrorCodes.ShowTimeConflict, [conflictingShowTimes],
                HttpStatusCode.Conflict);
        }

        Schedule schedule = new Schedule(request.MovieId, hallId, showTime,
            new Price(request.Price.Amount, request.Price.Currency));
        await scheduleRepository.AddAsync(schedule);

        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }

    public async Task<AppResult<List<GetMoviesByHallIdResponse>>> GetMoviesByHallId(Guid hallId)
    {
        List<Schedule> schedules = await scheduleRepository.GetMoviesByHallIdAsync(hallId);

        List<GetMoviesByHallIdResponse> response = schedules
            .Select(x => new GetMoviesByHallIdResponse(x.Id, x.MovieId, x.ShowTime.StartTime, x.ShowTime.EndTime))
            .ToList();

        return AppResult<List<GetMoviesByHallIdResponse>>.SuccessAsOk(response);
    }

    Task<AppResult> IScheduleAppService.AddMovieToHall(Guid hallId, AddMovieToHallRequest request)
    {
        throw new NotImplementedException();
    }

    Task<AppResult<List<GetMoviesByHallIdResponse>>> IScheduleAppService.GetMoviesByHallId(Guid hallId)
    {
        throw new NotImplementedException();
    }
}