namespace CinemaTicketingSystem.Application.Abstraction.Schedule;

public record GetMoviesByHallIdResponse(Guid ScheduleId, Guid MovieId, TimeOnly Start, TimeOnly End);