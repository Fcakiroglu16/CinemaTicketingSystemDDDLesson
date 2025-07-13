namespace CinemaTicketingSystem.Domain.Core;

public class BusinessException : Exception
{

    public BusinessException(string message, string errorCode) : base(message)
    {

        ErrorCode = errorCode;
    }

    public BusinessException(string message, string errorCode, Exception innerException) : base(message,
        innerException)
    {

        ErrorCode = errorCode;
    }

    public string ErrorCode { get; }

}