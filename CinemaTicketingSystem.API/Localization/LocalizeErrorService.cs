#region

using System.Net;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;

#endregion

namespace CinemaTicketingSystem.API.Localization;

public class LocalizeErrorService(ILocalizer localizer) : ILocalizeErrorService, IScopedDependency

{
    public AppResult Error(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle);

        return AppResult.Error(titleError, httpStatusCode);
    }

    public AppResult Error(string ErrorCodeAsTitle, string ErrorCodeAsDescription,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle);
        var descriptionError = LocalizeError(ErrorCodeAsDescription);

        return AppResult.Error(titleError, descriptionError, httpStatusCode);
    }

    public AppResult Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);

        return AppResult.Error(titleError, statusCode);
    }


    public AppResult Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        string ErrorCodeAsDescription, object[]? ErrorCodeAsDescriptionPlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);
        var descriptionError = LocalizeError(ErrorCodeAsDescription, ErrorCodeAsDescriptionPlaceHolder);

        return AppResult.Error(titleError, descriptionError, statusCode);
    }


    public AppResult<T> Error<T>(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle);

        return AppResult<T>.Error(titleError, httpStatusCode);
    }

    public AppResult<T> Error<T>(string ErrorCodeAsTitle, string ErrorCodeAsDescription,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle);
        var descriptionError = LocalizeError(ErrorCodeAsDescription);

        return AppResult<T>.Error(titleError, descriptionError, httpStatusCode);
    }

    public AppResult<T> Error<T>(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);

        return AppResult<T>.Error(titleError, statusCode);
    }


    public AppResult<T> Error<T>(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        string ErrorCodeAsDescription, object[]? ErrorCodeAsDescriptionPlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);
        var descriptionError = LocalizeError(ErrorCodeAsDescription, ErrorCodeAsDescriptionPlaceHolder);

        return AppResult<T>.Error(titleError, descriptionError, statusCode);
    }

    private string LocalizeError(string errorCode, object[]? data = null)
    {
        return localizer.L(errorCode, data);
    }
}