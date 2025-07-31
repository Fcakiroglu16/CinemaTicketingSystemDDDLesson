namespace CinemaTicketingSystem.Application.Abstraction.Ticketing;

public record PurchaseTicketResponse(
    string CinemaName,
    string HallName,
    string MovieTitle,
    DateTime ShowTime,
    List<SeatDto> Seats);