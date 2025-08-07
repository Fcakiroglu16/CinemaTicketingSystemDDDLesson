#region

using CinemaTicketingSystem.Application.Abstraction.Ticketing;

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Schedule;

public record AddMovieToHallRequest(Guid MovieId, TimeOnly StartTime, TimeOnly? EndTime, PriceDto Price);