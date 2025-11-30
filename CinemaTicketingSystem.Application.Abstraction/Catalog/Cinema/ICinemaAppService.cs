#region

#endregion

using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema.Hall;

namespace CinemaTicketingSystem.Application.Contracts.Catalog.Cinema;

public interface ICinemaAppService
{
    Task<AppResult> CreateAsync(CreateCinemaRequest request);
    Task<AppResult> AddHallAsync(Guid cinemaId, AddCinemaHallRequest request);
    Task<AppResult> RemoveHallAsync(RemoveCinemaHallRequest request);
    Task<AppResult<List<CinemaHallDto>>> GetCinemaHallsAsync(Guid cinemaId);
    Task<AppResult<CinemaDto>> GetAsync(Guid cinemaId);
    Task<AppResult<List<CinemaDto>>> GetAllAsync();
}