#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Schedule;

public record AddMovieToHallRequest(Guid MovieId, TimeOnly StartTime, TimeOnly? EndTime, PriceDto Price);