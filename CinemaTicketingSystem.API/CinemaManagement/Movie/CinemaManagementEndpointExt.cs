using Asp.Versioning.Builder;
using CinemaTicketingSystem.API.CinemaManagement.Cinema.Create;
using CinemaTicketingSystem.API.CinemaManagement.Movie.Create;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CinemaTicketingSystem.API.CinemaManagement.Movie;

public static class CinemaManagementEndpointExt
{
    public static void AddCinemaManagementGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/cinema-management").WithTags("cinema-management")
            .WithApiVersionSet(apiVersionSet)
            .CreateMovieGroupItemEndpoint()
            .CreateCinemaGroupItemEndpoint();
    }
}