using System.Net;
using System.Text.Json.Serialization;

namespace CinemaTicketingSystem.Application.Abstraction;

public class AppResult
{
    [JsonIgnore] public HttpStatusCode Status { get; set; }

    public AppProblemDetails? ProblemDetails { get; set; }

    [JsonIgnore] public bool IsSuccess => ProblemDetails is null;
    [JsonIgnore] public bool IsFail => !IsSuccess;


    // Static factory methods
    public static AppResult SuccessAsNoContent()
    {
        return new AppResult
        {
            Status = HttpStatusCode.NoContent
        };
    }

    public static AppResult ErrorAsNotFound()
    {
        return new AppResult
        {
            Status = HttpStatusCode.NotFound,
            ProblemDetails = new AppProblemDetails
            {
                Title = "Not Found",
                Detail = "The requested resource was not found"
            }
        };
    }


    public static AppResult Error(AppProblemDetails problemDetails, HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new AppResult
        {
            Status = status,
            ProblemDetails = problemDetails
        };
    }

    public static AppResult Error(string title, string description, HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new AppResult
        {
            Status = status,
            ProblemDetails = new AppProblemDetails
            {
                Title = title,
                Detail = description,
                Status = status.GetHashCode()
            }
        };
    }

    public static AppResult Error(string title, HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new AppResult
        {
            Status = status,
            ProblemDetails = new AppProblemDetails
            {
                Title = title,
                Status = status.GetHashCode()
            }
        };
    }


    public static AppResult ErrorFromValidation(IDictionary<string, object?> errors)
    {
        return new AppResult
        {
            Status = HttpStatusCode.BadRequest,
            ProblemDetails = new AppProblemDetails
            {
                Title = "Validation errors occured",
                Detail = "Please check the errors property for more details",
                Extensions = errors,
                Status = HttpStatusCode.BadRequest.GetHashCode()
            }
        };
    }
}

public class AppResult<T> : AppResult
{
    public T? Data { get; set; }

    [JsonIgnore] public string? UrlAsCreated { get; set; }

    public static AppResult<T> SuccessAsOk(T data)
    {
        return new AppResult<T>
        {
            Status = HttpStatusCode.OK,
            Data = data
        };
    }

    //201 => Created => respones body header => location== api/products/5
    public static AppResult<T> SuccessAsCreated(T data, string url)
    {
        return new AppResult<T>
        {
            Status = HttpStatusCode.Created,
            Data = data,
            UrlAsCreated = url
        };
    }

    public new static AppResult<T> Error(AppProblemDetails problemDetails,
        HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new AppResult<T>
        {
            Status = status,
            ProblemDetails = problemDetails
        };
    }

    public new static AppResult<T> Error(string title, string description,
        HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new AppResult<T>
        {
            Status = status,
            ProblemDetails = new AppProblemDetails
            {
                Title = title,
                Detail = description,
                Status = status.GetHashCode()
            }
        };
    }

    public new static AppResult<T> Error(string title, HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new AppResult<T>
        {
            Status = status,
            ProblemDetails = new AppProblemDetails
            {
                Title = title,
                Status = status.GetHashCode()
            }
        };
    }


    public new static AppResult<T> ErrorFromValidation(IDictionary<string, object?> errors)
    {
        return new AppResult<T>
        {
            Status = HttpStatusCode.BadRequest,
            ProblemDetails = new AppProblemDetails
            {
                Title = "Validation errors occured",
                Detail = "Please check the errors property for more details",
                Extensions = errors,
                Status = HttpStatusCode.BadRequest.GetHashCode()
            }
        };
    }
}