#region

#endregion

namespace CinemaTicketingSystem.SharedKernel.Exceptions;

public class SeatNotFoundException : BusinessException
{
    public SeatNotFoundException(string row, int number)
        : base(ErrorCodes.SeatNotFound)
    {
        AddData(row);
        AddData(number.ToString());
    }
}