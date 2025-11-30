namespace CinemaTicketingSystem.Application.Contracts.Schedule;

public record GetMoviesByHallIdResponse(Guid ScheduleId, Guid MovieId, TimeOnly Start, TimeOnly End);