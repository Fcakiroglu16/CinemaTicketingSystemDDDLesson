using CinemaTicketingSystem.Application.Abstraction;

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public record PurchaseTicketRequest(
    List<SeatPositionDto> SeatPositionList,
    Guid ScheduledMovieShowId,
    DateOnly ScreeningDate);