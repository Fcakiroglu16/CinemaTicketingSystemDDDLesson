namespace CinemaTicketingSystem.Application.Abstraction.Ticketing;

public record PurchaseTicketRequest(List<SeatPositionDto> SeatPositionList, Guid ScheduledMovieShowId);