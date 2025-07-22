namespace CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;

public record CreateCinemaRequest(string Name, AddressDto Address);