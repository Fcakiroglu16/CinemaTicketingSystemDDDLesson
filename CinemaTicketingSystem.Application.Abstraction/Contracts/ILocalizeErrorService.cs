#region

using System.Net;

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Contracts;

public interface ILocalizeErrorService
{
    AppResult Error(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest);

    AppResult Error(string ErrorCodeAsTitle, string ErrorCodeAsDescription,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest);

    AppResult Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest);

    AppResult Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder, string ErrorCodeAsDescription,
        object[]? ErrorCodeAsDescriptionPlaceHolder, HttpStatusCode statusCode = HttpStatusCode.BadRequest);

    AppResult<T> Error<T>(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest);

    AppResult<T> Error<T>(string ErrorCodeAsTitle, string ErrorCodeAsDescription,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest);

    AppResult<T> Error<T>(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest);

    AppResult<T> Error<T>(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder, string ErrorCodeAsDescription,
        object[]? ErrorCodeAsDescriptionPlaceHolder, HttpStatusCode statusCode = HttpStatusCode.BadRequest);
}