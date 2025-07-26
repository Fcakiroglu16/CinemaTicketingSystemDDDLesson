namespace CinemaTicketingSystem.Application.Abstraction.Schedule;

public record AddMovieToHallRequest(Guid MovieId, TimeOnly StartTime, TimeOnly? EndTime);