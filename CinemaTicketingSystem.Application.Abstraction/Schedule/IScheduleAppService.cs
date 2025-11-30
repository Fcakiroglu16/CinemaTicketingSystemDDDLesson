#region

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Schedule;

public interface IScheduleAppService
{
    Task<AppResult> AddMovieToHall(Guid hallId, AddMovieToHallRequest request);

    Task<AppResult<List<GetMoviesByHallIdResponse>>> GetMoviesByHallId(Guid hallId);
}