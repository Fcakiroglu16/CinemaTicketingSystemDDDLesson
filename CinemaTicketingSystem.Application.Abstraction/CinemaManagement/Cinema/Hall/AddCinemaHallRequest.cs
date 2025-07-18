using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema.Hall
{
    public record AddCinemaHallRequest(Guid CinemaId, string Name, int[] Technologies, List<SeatDto> SeatList)
    {
    }

    public record SeatDto(string Row, int Number, SeatType seatType)
    {
    }
}
