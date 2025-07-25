namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class SeatNotFoundException : DomainException
{
    public SeatNotFoundException(string row, int number)
        : base(ErrorCodes.SeatNotFound)
    {

        AddData(row);
        AddData(number.ToString());

    }


}