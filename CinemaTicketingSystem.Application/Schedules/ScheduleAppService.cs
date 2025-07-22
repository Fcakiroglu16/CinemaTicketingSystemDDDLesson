using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Domain.Scheduling;

namespace CinemaTicketingSystem.Application.Schedules;

internal class ScheduleAppService(
    IGenericRepository<Guid, CinemaHallSnapshot> cinemeHallScheduleRepository,
    IGenericRepository<Guid, MovieSnapshot> movieScheduleRepository) : IScopedDependency
{
    public async Task<AppResult> AddMovieToHall(Guid HallId, Guid MovieId, TimeSpan startTime, TimeSpan endTime)
    {
        var movieSchedule = await movieScheduleRepository.GetByIdAsync(MovieId);
        var hallSchedule = await cinemeHallScheduleRepository.GetByIdAsync(HallId);


        //if (movieSchedule == null || hallSchedule == null)
        //{
        //    return AppResult.ErrorAsNotFound();
        //}


        //var showTime = new ShowTime(startTime, endTime);

        //movieSchedule.AddShowTime(showTime);


        return AppResult.SuccessAsNoContent();
    }
}