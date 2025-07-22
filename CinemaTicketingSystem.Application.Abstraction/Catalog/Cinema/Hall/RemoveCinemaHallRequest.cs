namespace CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema.Hall;

public record RemoveCinemaHallRequest(Guid CinemaId, Guid HallId)
{
}