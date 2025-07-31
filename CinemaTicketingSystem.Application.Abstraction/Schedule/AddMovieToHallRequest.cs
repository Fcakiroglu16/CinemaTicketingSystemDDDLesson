using CinemaTicketingSystem.Application.Abstraction.Ticketing;

namespace CinemaTicketingSystem.Application.Abstraction.Schedule;

public record AddMovieToHallRequest(Guid MovieId, TimeOnly StartTime, TimeOnly? EndTime, PriceDto Price);