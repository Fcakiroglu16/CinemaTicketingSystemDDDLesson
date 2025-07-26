using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketingSystem.API.Extensions;

public class UserFriendlyExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UserFriendlyException userFriendlyException) return false;
        var problemDetails = new ProblemDetails();

        var errorCode = userFriendlyException.ErrorCode;
        var placeHolderList = userFriendlyException.PlaceholderData;

        var localizer = httpContext.RequestServices.GetRequiredService<ILocalizer>();

        var title = placeHolderList.Any() ? localizer.L(errorCode, placeHolderList.ToArray()) : localizer.L(errorCode);


        httpContext.Response.StatusCode = userFriendlyException.StatusCode.GetHashCode();
        problemDetails.Title = title;
        problemDetails.Status = userFriendlyException.StatusCode.GetHashCode();
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);


        return true;
    }
}