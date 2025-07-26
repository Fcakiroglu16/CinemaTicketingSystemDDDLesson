namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class DomainException(string errorCode) : Exception
{
    private readonly List<string> PlaceHolderData = [];
    public string? ErrorCode { get; private set; } = errorCode;

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