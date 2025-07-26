using System.Net;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Application;

public class AppDependencyService(IUnitOfWork unitOfWork, ILocalizer localizer)
{
    public IUnitOfWork UnitOfWork => unitOfWork;

    private string LocalizeError(string ErrorCodeAsTitle, params object[]? data)
    {
        return data is not null ? localizer.L(ErrorCodeAsTitle, data) : localizer.L(ErrorCodeAsTitle);
    }

    public AppResult Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder, HttpStatusCode statusCode)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);

        return AppResult.Error(titleError, statusCode);
    }


    public AppResult Error(string ErrorCodeAsTitle, object[]? data)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, data);

        return AppResult.Error(titleError);
    }

    public AppResult Error(string ErrorCodeAsTitle)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, null);

        return AppResult.Error(titleError);
    }

    public AppResult Error(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, null);

        return AppResult.Error(titleError, httpStatusCode);
    }

    public AppResult<T> Error<T>(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        HttpStatusCode statusCode)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);

        return AppResult<T>.Error(titleError, statusCode);
    }


    public AppResult<T> Error<T>(string ErrorCodeAsTitle, object[]? data)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, data);

        return AppResult<T>.Error(titleError);
    }

    public AppResult<T> Error<T>(string ErrorCodeAsTitle)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, null);

        return AppResult<T>.Error(titleError);
    }


    public AppResult<T> Error<T>(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, null);

        return AppResult<T>.Error(titleError, httpStatusCode);
    }

    public AppResult Error(string ErrorCodeAsTitle, object[]? errorCodeAsTitlePlaceHolder,
        string errorCodeAsDescription, object[]? errorCodeAsDescriptionPlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, errorCodeAsTitlePlaceHolder);
        var descriptionError = LocalizeError(errorCodeAsDescription, errorCodeAsDescriptionPlaceHolder);


        return AppResult.Error(titleError, descriptionError, statusCode);
    }

    public AppResult<T> Error<T>(string ErrorCodeAsTitle, object[]? errorCodeAsTitlePlaceHolder,
        string errorCodeAsDescription, object[]? errorCodeAsDescriptionPlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, errorCodeAsTitlePlaceHolder);
        var descriptionError = LocalizeError(errorCodeAsDescription, errorCodeAsDescriptionPlaceHolder);


        return AppResult<T>.Error(titleError, descriptionError, statusCode);
    }
}