using CinemaTicketingSystem.Application.Abstraction;

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public record CreateSeatHoldRequest(
    List<SeatPositionDto> SeatPositions,
    Guid ScheduledMovieShowId,
    DateOnly ScreeningDate);