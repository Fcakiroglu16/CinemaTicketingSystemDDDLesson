using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketingSystem.API.Extensions;

public class BusinessExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BusinessException domainException) return false;
        var problemDetails = new ProblemDetails();

        var errorCode = domainException.ErrorCode;
        var placeHolderList = domainException.PlaceholderData;

        var localizer = httpContext.RequestServices.GetRequiredService<ILocalizer>();

        var title = placeHolderList.Any() ? localizer.L(errorCode, placeHolderList.ToArray()) : localizer.L(errorCode);

        problemDetails.Title = title;
        problemDetails.Status = StatusCodes.Status400BadRequest;
        ;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);


        return true;
    }
}