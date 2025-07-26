namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class SeatAlreadyExistsException : DomainException
{
    public SeatAlreadyExistsException(string row, int number)
        : base(ErrorCodes.SeatAlreadyExists)
    {
        AddData(row);

        AddData(number.ToString());
    }
}