namespace CinemaTicketingSystem.Application.Abstraction.Ticketing;

public record CreateSeatHoldRequest(List<SeatPositionDto> SeatPosition, Guid ScheduledMovieShowId);