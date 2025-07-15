using CinemaTicketingSystem.Application.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CinemaTicketingSystem.API.Extensions
{
    public static class EndpointResultExt
    {
        public static IResult ToGenericResult<T>(this AppResult<T> result)
        {
            return result.Status switch
            {
                HttpStatusCode.OK => Results.Ok(result.Data),
                HttpStatusCode.Created => Results.Created(result.UrlAsCreated, result.Data),
                HttpStatusCode.NotFound => Results.NotFound(result.ProblemDetails!),
                _ => Results.Problem(new ProblemDetails()
                {
                    Title = result.ProblemDetails!.Title,
                    Detail = result.ProblemDetails.Detail,
                    Status = result.ProblemDetails.Status,
                    Extensions = result.ProblemDetails.Extensions!

                })
            };
        }

        public static IResult ToGenericResult(this AppResult result)
        {
            return result.Status switch
            {
                HttpStatusCode.NoContent => Results.NoContent(),
                HttpStatusCode.NotFound => Results.NotFound(result.ProblemDetails!),
                _ => Results.Problem(new ProblemDetails()
                {
                    Title = result.ProblemDetails!.Title,
                    Detail = result.ProblemDetails.Detail,
                    Status = result.ProblemDetails.Status,
                    Extensions = result.ProblemDetails.Extensions!

                })
            };
        }
    }
}