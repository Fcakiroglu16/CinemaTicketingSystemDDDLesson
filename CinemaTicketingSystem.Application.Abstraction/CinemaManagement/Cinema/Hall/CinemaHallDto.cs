namespace CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema.Hall
{
    public record CinemaHallDto(string Name, int[] SupportedTechnologies, List<SeatDto> Seats);
}
