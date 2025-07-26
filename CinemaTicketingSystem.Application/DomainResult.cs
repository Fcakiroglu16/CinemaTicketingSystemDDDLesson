namespace CinemaTicketingSystem.Application;

public class DomainResult
{
    private DomainResult()
    {
    }


    private DomainResult(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    private DomainResult(bool isSuccess, string error, params object[] errorData)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorData = errorData;
    }

    public bool IsSuccess { get; private set; }
    public string? Error { get; private set; }
    public object[]? ErrorData { get; private set; }

    public static DomainResult Success()
    {
        return new DomainResult(true, string.Empty);
    }

    public static DomainResult Failure(string error)
    {
        return new DomainResult(false, error);
    }

    public static DomainResult Failure(string error, params object[] errorData)
    {
        return new DomainResult(false, error, errorData);
    }
}