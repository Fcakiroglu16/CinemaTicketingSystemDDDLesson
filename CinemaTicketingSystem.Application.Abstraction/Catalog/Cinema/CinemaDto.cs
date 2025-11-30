namespace CinemaTicketingSystem.Application.Contracts.Catalog.Cinema;

public record CinemaDto(Guid Id, string Name, AddressDto Address);