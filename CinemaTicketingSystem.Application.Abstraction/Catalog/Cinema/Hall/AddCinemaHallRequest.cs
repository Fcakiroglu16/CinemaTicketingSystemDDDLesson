using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema.Hall;

public record AddCinemaHallRequest(string Name, int[] Technologies, List<SeatDto> SeatList)
{
}

public record SeatDto(string Row, int Number, SeatType SeatType);