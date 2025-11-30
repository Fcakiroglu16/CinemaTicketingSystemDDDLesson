#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Extensions;

public class BusinessExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BusinessException domainException) return false;
        ProblemDetails problemDetails = new ProblemDetails();

        string errorCode = domainException.ErrorCode;
        IReadOnlyList<object> placeHolderList = domainException.PlaceholderData;

        ILocalizer localizer = httpContext.RequestServices.GetRequiredService<ILocalizer>();

        string title = placeHolderList.Any() ? localizer.L(errorCode, placeHolderList.ToArray()) : localizer.L(errorCode);

        problemDetails.Title = title;
        problemDetails.Status = HttpStatusCode.BadRequest.GetHashCode();
        httpContext.Response.StatusCode = HttpStatusCode.BadRequest.GetHashCode();

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);


        return true;
    }
}