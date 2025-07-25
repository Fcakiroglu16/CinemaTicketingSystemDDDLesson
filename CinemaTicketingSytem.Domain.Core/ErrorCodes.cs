namespace CinemaTicketingSystem.Domain.Core;

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
    public const string IMAXRequiresMinimumSeats = "Schedule:IMAXRequiresMinimumSeats";
    public const string MovieDurationMismatch = "Schedule:MovieDurationMismatch";
    public const string ShowTimeConflict = "Schedule:ShowTimeConflict";
}