namespace CinemaTicketingSystem.Domain.Core;

public static class TicketingErrorCodes
{
    public const string DuplicateSeatErrorCode = "Ticketing.DuplicateSeat";
    public const string MaxTicketLimitExceeded = "Ticketing.MaxTicketLimitExceeded";
    public const string TicketAlreadyUsed = "Ticketing.TicketAlreadyUsed";
    public const string TicketNotFound = "Ticketing.TicketNotFound";


    public const string DuplicateReservedSeat = "Reservations.DuplicateReservedSeat";
    public const string EmptyReservation = "Reservations.EmptyReservation";
    public const string InvalidReservationState = "Reservations.InvalidState";
    public const string MaxSeatLimitExceeded = "Reservations.MaxSeatLimitExceeded";
    public const string ReservationExpired = "Reservations.ReservationExpired";
    public const string ReservedSeatNotFound = "Reservations.ReservedSeatNotFound";
}