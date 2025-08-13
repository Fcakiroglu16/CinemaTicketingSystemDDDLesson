using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public interface ISeatHoldAppService
{
    Task<AppResult> CreateAsync(CreateSeatHoldRequest request);
    Task<AppResult> Cancel();
}