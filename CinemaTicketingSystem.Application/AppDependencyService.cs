using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.SharedKernel;
using System.Net;

namespace CinemaTicketingSystem.Application;

public class AppDependencyService(IUnitOfWork unitOfWork, ILocalizer localizer, IUserContext userContext)
{
    public IUnitOfWork UnitOfWork => unitOfWork;
    public IUserContext UserContext => userContext;

    private string LocalizeError(string errorCode, object[]? data = null)
    {
        return localizer.L(errorCode, data);
    }

    public AppResult Error(string ErrorCodeAsTitle, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle);

        return AppResult.Error(titleError, httpStatusCode);
    }
    public AppResult Error(string ErrorCodeAsTitle, string ErrorCodeAsDescription, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle);
        var descriptionError = LocalizeError(ErrorCodeAsDescription);

        return AppResult.Error(titleError, descriptionError, httpStatusCode);
    }

    public AppResult Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);

        return AppResult.Error(titleError, statusCode);
    }


    public AppResult Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder, string ErrorCodeAsDescription, object[]? ErrorCodeAsDescriptionPlaceHolder, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);
        var descriptionError = LocalizeError(ErrorCodeAsDescription, ErrorCodeAsDescriptionPlaceHolder);

        return AppResult.Error(titleError, descriptionError, statusCode);
    }
    public AppResult Error(string ErrorCodeAsTitle, object[]? ErrorCodeAsTitlePlaceHolder, string ErrorCodeAsDescription, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var titleError = LocalizeError(ErrorCodeAsTitle, ErrorCodeAsTitlePlaceHolder);
        var descriptionError = LocalizeError(ErrorCodeAsDescription);

        return AppResult.Error(titleError, descriptionError, statusCode);
    }


}