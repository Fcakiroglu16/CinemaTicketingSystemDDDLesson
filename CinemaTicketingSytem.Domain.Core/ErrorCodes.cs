namespace CinemaTicketingSystem.SharedKernel;

public static class ErrorCodes
{
    public const string CinemaAlreadyExists = "Catalog:CinemaAlreadyExists";
    public const string CinemaHallAlreadyExists = "Catalog:CinemaHallAlreadyExists";
    public const string CinemaHallNotFound = "Catalog:CinemaHallNotFound";
    public const string SeatAlreadyExists = "Catalog:SeatAlreadyExists";
    public const string SeatNotFound = "Catalog:SeatNotFound";
    public const string CinemaNotFound = "Catalog:CinemaNotFound";
    public const string MovieNotFound = "Catalog:MovieNotFound";
    public const string MovieAlreadyExists = "Catalog:MovieAlreadyExists";

    public const string HallTechnologyNotSupported = "Schedule:HallTechnologyNotSupported";
    public const string ImaxRequiresMinimumSeats = "Schedule:IMAXRequiresMinimumSeats";
    public const string MovieDurationMismatch = "Schedule:MovieDurationMismatch";
    public const string ShowTimeConflict = "Schedule:ShowTimeConflict";
    public const string ScheduleNotFound = "Schedule:ScheduleNotFound";


    public const string MaxTicketsExceeded = "Ticketing:MaxTicketsExceeded";
    public const string TicketNotFound = "Ticketing:TicketNotFound";
    public const string DuplicateSeat = "Ticketing:DuplicateSeat";
    public const string TicketAlreadyUsed = "Ticketing:TicketAlreadyUsed";
    public const string TicketNotAvailable = "Ticketing:TicketNotAvailable";
    public const string SeatNotAvailable = "Ticketing:SeatNotAvailable";
    public const string NotEnoughSeatsAvailable = "Ticketing:NotEnoughSeatsAvailable";
    public const string MaxSeatsPerReservation = "Ticketing:MaxSeatsPerReservation";
    public const string SeatAlreadyReserved = "Ticketing:SeatAlreadyReserved";
    public const string ReservedSeatNotFound = "Ticketing:ReservedSeatNotFound";
    public const string NoSeatsReserved = "Ticketing:NoSeatsReserved";
    public const string CannotCancelExpiredReservation = "Ticketing:CannotCancelExpiredReservation";
    public const string SeatHoldExpired = "Ticketing:SeatHoldExpired";
    public const string SeatAlreadyHeld = "Ticketing:SeatAlreadyHeld";

    public const string SeatHoldNotFound = "Ticketing:SeatHoldNotFound";
    public const string ReservationTooLate = "Ticketing:ReservationTooLate";
    public const string ReservationExpired = "Ticketing:ReservationExpired";


    public const string InvalidCredentials = "Account.InvalidCredentials";
    public const string EmailOrPasswordWrong = "Account.EmailOrPasswordWrong";
    public const string UserAlreadyExists = "Account:UserAlreadyExists";
    public const string UserNotFound = "Account:UserNotFound";


    public const string ServerErrorTitle = "Common:ServerErrorTitle";
    public const string ServerErrorDetail = "Common:ServerErrorDetail";
}