namespace CinemaTicketingSystem.Application.Contracts.Catalog.Cinema.Hall;

public record CinemaHallDto(Guid Id, string Name, int[] SupportedTechnologies, List<SeatDto> Seats);