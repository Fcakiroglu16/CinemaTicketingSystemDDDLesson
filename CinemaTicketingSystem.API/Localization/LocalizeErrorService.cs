#region

using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using System.Net;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Localization;

public class LocalizeErrorService(ILocalizer localizer) : ILocalizeErrorService, IScopedDependency

{
    public AppResult Error(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        string titleError = LocalizeError(ErrorCodeAsTitle);

        return AppResult.Error(titleError, httpStatusCode);
    }

    public AppResult Error(string ErrorCodeAsTitle, string ErrorCodeAsDescription,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        string titleError = LocalizeError(ErrorCodeAsTitle);
        string descriptionError = LocalizeError(ErrorCodeAsDescription);

        return AppResult.Error(titleError, descriptionError, httpStatusCode);
    }

    public AppResult Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        string titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);

        return AppResult.Error(titleError, statusCode);
    }


    public AppResult Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        string ErrorCodeAsDescription, object[]? ErrorCodeAsDescriptionPlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        string titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);
        string descriptionError = LocalizeError(ErrorCodeAsDescription, ErrorCodeAsDescriptionPlaceHolder);

        return AppResult.Error(titleError, descriptionError, statusCode);
    }


    public AppResult<T> Error<T>(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        string titleError = LocalizeError(ErrorCodeAsTitle);

        return AppResult<T>.Error(titleError, httpStatusCode);
    }

    public AppResult<T> Error<T>(string ErrorCodeAsTitle, string ErrorCodeAsDescription,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        string titleError = LocalizeError(ErrorCodeAsTitle);
        string descriptionError = LocalizeError(ErrorCodeAsDescription);

        return AppResult<T>.Error(titleError, descriptionError, httpStatusCode);
    }

    public AppResult<T> Error<T>(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        string titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);

        return AppResult<T>.Error(titleError, statusCode);
    }


    public AppResult<T> Error<T>(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        string ErrorCodeAsDescription, object[]? ErrorCodeAsDescriptionPlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        string titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);
        string descriptionError = LocalizeError(ErrorCodeAsDescription, ErrorCodeAsDescriptionPlaceHolder);

        return AppResult<T>.Error(titleError, descriptionError, statusCode);
    }

    AppResult ILocalizeErrorService.Error(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode)
    {
        throw new NotImplementedException();
    }

    AppResult ILocalizeErrorService.Error(string ErrorCodeAsTitle, string ErrorCodeAsDescription, HttpStatusCode httpStatusCode)
    {
        throw new NotImplementedException();
    }

    AppResult ILocalizeErrorService.Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder, HttpStatusCode statusCode)
    {
        throw new NotImplementedException();
    }

    AppResult ILocalizeErrorService.Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder, string ErrorCodeAsDescription, object[]? ErrorCodeAsDescriptionPlaceHolder, HttpStatusCode statusCode)
    {
        throw new NotImplementedException();
    }

    AppResult<T> ILocalizeErrorService.Error<T>(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode)
    {
        throw new NotImplementedException();
    }

    AppResult<T> ILocalizeErrorService.Error<T>(string ErrorCodeAsTitle, string ErrorCodeAsDescription, HttpStatusCode httpStatusCode)
    {
        throw new NotImplementedException();
    }

    AppResult<T> ILocalizeErrorService.Error<T>(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder, HttpStatusCode statusCode)
    {
        throw new NotImplementedException();
    }

    AppResult<T> ILocalizeErrorService.Error<T>(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder, string ErrorCodeAsDescription, object[]? ErrorCodeAsDescriptionPlaceHolder, HttpStatusCode statusCode)
    {
        throw new NotImplementedException();
    }

    private string LocalizeError(string errorCode, object[]? data = null)
    {
        return localizer.L(errorCode, data);
    }
}