#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Extensions;

public class UserFriendlyExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UserFriendlyException userFriendlyException) return false;
        ProblemDetails problemDetails = new ProblemDetails();

        string errorCode = userFriendlyException.ErrorCode;
        IReadOnlyList<string> placeHolderList = userFriendlyException.PlaceholderData;

        ILocalizer localizer = httpContext.RequestServices.GetRequiredService<ILocalizer>();

        string title = placeHolderList.Any() ? localizer.L(errorCode, placeHolderList.ToArray()) : localizer.L(errorCode);


        httpContext.Response.StatusCode = userFriendlyException.StatusCode.GetHashCode();
        problemDetails.Title = title;
        problemDetails.Status = userFriendlyException.StatusCode.GetHashCode();
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);


        return true;
    }
}