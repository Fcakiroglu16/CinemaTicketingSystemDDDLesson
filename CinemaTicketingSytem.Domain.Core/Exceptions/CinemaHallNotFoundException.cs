namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class CinemaHallNotFoundException : DomainException
{
    public CinemaHallNotFoundException(Guid hallId)
        : base(ErrorCodes.CinemaHallNotFound)
    {
        AddData(hallId.ToString());
    }


}