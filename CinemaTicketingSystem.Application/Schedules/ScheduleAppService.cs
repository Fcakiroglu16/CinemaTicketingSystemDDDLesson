using System.Net;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Application.Abstraction.Schedule;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Domain.Scheduling;
using CinemaTicketingSystem.Domain.Scheduling.Repositories;

namespace CinemaTicketingSystem.Application.Schedules;

public class ScheduleAppService(
    IGenericRepository<Guid, CinemaHallSnapshot> CinemaHallSnapshotRepository,
    IGenericRepository<Guid, MovieSnapshot> movieShotRepository,
    MovieHallCompatibilityService movieHallCompatibilityService,
    IScheduleRepository scheduleRepository,
    AppDependencyService appDependencyService) : IScopedDependency, IScheduleAppService
{
    public async Task<AppResult> AddMovieToHall(Guid hallId, AddMovieToHallRequest request)
    {
        var movie = await movieShotRepository.GetByIdAsync(request.MovieId);

        if (movie is null) return appDependencyService.Error(ErrorCodes.MovieNotFound);

        var hallSchedule = await CinemaHallSnapshotRepository.GetByIdAsync(hallId);

        if (hallSchedule is null) return appDependencyService.Error(ErrorCodes.CinemaHallNotFound);

        var compatibilityResult = movieHallCompatibilityService.IsCompatible(movie, hallSchedule);

        if (!compatibilityResult.IsSuccess)
            return appDependencyService.Error(compatibilityResult.Error!, compatibilityResult.ErrorData);

        ShowTime showTime;

        if (request.EndTime.HasValue)
        {
            var result = movie.IsValidDuration(request.StartTime, request.EndTime.Value);
            {
                if (!result) return appDependencyService.Error(ErrorCodes.MovieDurationMismatch);
            }

            showTime = ShowTime.Create(request.StartTime, request.EndTime.Value);
        }
        else
        {
            showTime = ShowTime.Create(request.StartTime, movie.Duration);
        }

        var schedules = (await scheduleRepository.WhereAsync(x => x.HallId == hallId)).ToList();

        if (schedules.Any(x => x.ShowTime.ConflictsWith(showTime)))
        {
            var conflictingShowTimes = string.Join(", ",
                schedules.Where(x => x.ShowTime.ConflictsWith(showTime))
                    .Select(x => x.ShowTime.GetDisplayInfo()));


            return appDependencyService.Error(ErrorCodes.ShowTimeConflict, [conflictingShowTimes],
                HttpStatusCode.Conflict);
        }

        var schedule = new Schedule(request.MovieId, hallId, showTime);
        await scheduleRepository.AddAsync(schedule);

        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }

    public async Task<AppResult<List<GetMoviesByHallIdRequest>>> GetMoviesByHallId(Guid hallId)
    {
        var schedules = await scheduleRepository.GetMoviesByHallIdAsync(hallId);

        var response = schedules
            .Select(x => new GetMoviesByHallIdRequest(x.MovieId, x.ShowTime.StartTime, x.ShowTime.EndTime)).ToList();

        return AppResult<List<GetMoviesByHallIdRequest>>.SuccessAsOk(response);
    }
}