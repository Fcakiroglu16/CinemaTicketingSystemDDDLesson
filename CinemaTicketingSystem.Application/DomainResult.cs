namespace CinemaTicketingSystem.Application;

public class DomainResult(bool isSuccess, string error)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public string Error { get; set; } = error;

    public static DomainResult Success()
    {
        return new DomainResult(true, string.Empty);
    }

    public static DomainResult Failure(string error)
    {
        return new DomainResult(false, error);
    }
}