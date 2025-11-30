namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public record PurchaseTicketResponse(
    string CinemaName,
    string HallName,
    string MovieTitle,
    DateTime ShowTime,
    List<SeatPositionDto> SeatPositionList);