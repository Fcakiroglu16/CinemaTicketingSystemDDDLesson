namespace CinemaTicketingSystem.Application.Abstraction.Schedule
{
    public record GetMoviesByHallId(Guid MovieId, TimeOnly Start, TimeOnly End);

}
