namespace CinemaTicketingSystem.Application.Abstraction.Schedule;

public interface IScheduleAppService
{
    Task<AppResult> AddMovieToHall(Guid hallId, AddMovieToHallRequest request);

    Task<AppResult<List<GetMoviesByHallId>>> GetMoviesByHallId(Guid hallId);
}