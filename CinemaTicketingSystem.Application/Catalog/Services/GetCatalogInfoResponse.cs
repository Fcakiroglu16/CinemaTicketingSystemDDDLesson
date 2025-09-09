namespace CinemaTicketingSystem.Application.Catalog.Services;

public record GetCatalogInfoResponse(string CinemaName, string HallName, string MovieTitle, short SeatCount);