using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Application.Abstraction.Schedule;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Domain.Scheduling;
using CinemaTicketingSystem.Domain.Scheduling.Repositories;
using System.Net;

namespace CinemaTicketingSystem.Application.Schedules;

public class ScheduleAppService(
    IGenericRepository<Guid, CinemaHallSnapshot> cinemaHallScheduleRepository,
    IGenericRepository<Guid, MovieSnapshot> movieScheduleRepository, MovieHallCompatibilityService movieHallCompatibilityService, IScheduleRepository scheduleRepository, IUnitOfWork unitOfWork) : IScopedDependency, IScheduleAppService
{
    public async Task<AppResult> AddMovieToHall(Guid hallId, AddMovieToHallRequest request)
    {
        var movie = await movieScheduleRepository.GetByIdAsync(request.MovieId);


        if (movie is null) return AppResult.Error("Movie not found", HttpStatusCode.NotFound);



        var hallSchedule = await cinemaHallScheduleRepository.GetByIdAsync(hallId);

        if (hallSchedule is null) return AppResult.Error("Hall not found", HttpStatusCode.NotFound);




        var compatibilityResult = movieHallCompatibilityService.IsCompatible(movie, hallSchedule);




        if (!compatibilityResult.IsSuccess)
            return AppResult.Error(compatibilityResult.Error, HttpStatusCode.BadRequest);



        ShowTime showTime;


        if (request.EndTime.HasValue)
        {


            if (!movie.IsValidDuration(request.StartTime, request.EndTime.Value))
            {

                return AppResult.Error("Movie duration is invalid for the given start and end times.",
                    HttpStatusCode.BadRequest);
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
            return AppResult.Error(
                $"Show time conflicts with existing schedule. Conflicting showtimes: {string.Join(", ", schedules.Where(x => x.ShowTime.ConflictsWith(showTime)).Select(x => x.ShowTime.GetDisplayInfo()))}",
                HttpStatusCode.Conflict);

        }

        var schedule = new Schedule(request.MovieId, hallId, showTime);
        await scheduleRepository.AddAsync(schedule);

        await unitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }

    public async Task<AppResult<List<GetMoviesByHallId>>> GetMoviesByHallId(Guid hallId)
    {

        var schedules = await scheduleRepository.GetMoviesByHallIdAsync(hallId);


        var response = schedules.Select(x => new GetMoviesByHallId(x.MovieId, x.ShowTime.StartTime, x.ShowTime.EndTime)).ToList();

        return AppResult<List<GetMoviesByHallId>>.SuccessAsOk(response);
    }

}