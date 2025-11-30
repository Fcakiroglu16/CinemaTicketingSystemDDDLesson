namespace CinemaTicketingSystem.Application.Contracts.Catalog.Cinema;

public record AddressDto(
    string Country,
    string City,
    string District,
    string Street,
    string PostalCode,
    string? Description = null);