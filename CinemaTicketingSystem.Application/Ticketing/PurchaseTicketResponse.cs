using CinemaTicketingSystem.Application.Abstraction.Ticketing;

namespace CinemaTicketingSystem.Application.Ticketing;

public record PurchaseTicketResponse(string CinemaName, string HallName, string MovieTitle,
    DateTime ShowTime,List<SeatDto> Seats);