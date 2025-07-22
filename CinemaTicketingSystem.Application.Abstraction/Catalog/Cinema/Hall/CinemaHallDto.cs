namespace CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema.Hall;

public record CinemaHallDto(Guid Id, string Name, int[] SupportedTechnologies, List<SeatDto> Seats);