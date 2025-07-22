namespace CinemaTicketingSystem.Application.Abstraction.Schedule
{
    internal record MovieDto(Guid MovieId, TimeOnly Start, TimeOnly End);
}
