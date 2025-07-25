namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class DomainException(string errorCode) : Exception
{
    public string? ErrorCode { get; private set; } = errorCode;

    private readonly List<string> PlaceHolderData = [];

    public IReadOnlyList<string> PlaceholderData => PlaceHolderData;


    public static DomainException Create(string errorCode)
    {

        return new DomainException(errorCode);
    }

    public DomainException AddData(string data)
    {
        PlaceHolderData.Add(data);
        return this;
    }


}

