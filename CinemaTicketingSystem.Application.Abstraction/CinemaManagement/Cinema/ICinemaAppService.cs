using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema.Hall;

namespace CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;

public interface ICinemaAppService
{
    Task<AppResult> CreateAsync(CreateCinemaRequest request);
    Task<AppResult> AddHallAsync(AddCinemaHallRequest request);
    Task<AppResult> RemoveHallAsync(RemoveCinemaHallRequest request);
    Task<AppResult<List<CinemaHallDto>>> GetCinemaHallsAsync(Guid cinemaId);
    Task<AppResult<CinemaDto>> GetAllAsync(Guid cinemaId);
    Task<AppResult<List<CinemaDto>>> GetAllAsync();
}