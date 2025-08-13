using CinemaTicketingSystem.Application.Abstraction;

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public interface IReservationAppService
{
    Task<AppResult> Create(ReserveSeatsRequest request);
}