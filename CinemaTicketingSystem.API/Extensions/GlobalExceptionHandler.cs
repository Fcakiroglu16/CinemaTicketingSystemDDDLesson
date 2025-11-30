#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.SharedKernel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Extensions;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        ILocalizer localizer = httpContext.RequestServices.GetRequiredService<ILocalizer>();

        string title = localizer.L(ErrorCodes.ServerErrorTitle);
        string detail = localizer.L(ErrorCodes.ServerErrorDetail);


        ProblemDetails problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = title,
            Detail = detail
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}