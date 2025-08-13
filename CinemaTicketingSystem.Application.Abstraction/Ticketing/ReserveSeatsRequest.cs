using CinemaTicketingSystem.Application.Abstraction;

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public record ReserveSeatsRequest(
    List<SeatPositionDto> SeatPositionList,
    Guid ScheduledMovieShowId,
    DateOnly ScreeningDate);