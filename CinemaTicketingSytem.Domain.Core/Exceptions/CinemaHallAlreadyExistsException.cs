namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class CinemaHallAlreadyExistsException : DomainException
{
    public CinemaHallAlreadyExistsException(string cinemaHallName) : base(ErrorCodes.CinemaHallAlreadyExists)
    {
        AddData(cinemaHallName);
    }
}