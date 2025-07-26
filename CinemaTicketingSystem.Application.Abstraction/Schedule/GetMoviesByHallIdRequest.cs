namespace CinemaTicketingSystem.Application.Abstraction.Schedule;

public record GetMoviesByHallIdRequest(Guid MovieId, TimeOnly Start, TimeOnly End);