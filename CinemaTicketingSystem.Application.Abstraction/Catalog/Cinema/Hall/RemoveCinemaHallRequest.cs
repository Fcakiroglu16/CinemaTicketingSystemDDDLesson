namespace CinemaTicketingSystem.Application.Contracts.Catalog.Cinema.Hall;

public record RemoveCinemaHallRequest(Guid CinemaId, Guid HallId)
{
}