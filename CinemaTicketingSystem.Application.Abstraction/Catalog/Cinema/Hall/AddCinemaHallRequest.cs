#region

#endregion

using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Application.Contracts.Catalog.Cinema.Hall;

public record AddCinemaHallRequest(string Name, int[] Technologies, List<SeatDto> SeatList)
{
}

public record SeatDto(string Row, int Number, SeatType SeatType);