namespace CinemaTicketingSystem.Application.Abstraction.Ticketing;

public record PurchaseTicketRequest(List<SeatDto> seats, Guid ScheduleId);