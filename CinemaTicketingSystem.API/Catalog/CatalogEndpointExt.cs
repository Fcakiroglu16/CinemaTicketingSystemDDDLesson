using Asp.Versioning.Builder;
using CinemaTicketingSystem.API.Catalog.Cinema.Create;
using CinemaTicketingSystem.API.Catalog.Cinema.GetAll;
using CinemaTicketingSystem.API.Catalog.Cinema.Hall.Add;
using CinemaTicketingSystem.API.Catalog.Movie.Create;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CinemaTicketingSystem.API.Catalog;

public static class CatalogEndpointExt
{
    public static void AddCatalogGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/catalogs").WithTags("catalogs")
            .WithApiVersionSet(apiVersionSet)
            .CreateMovieGroupItemEndpoint()
            .CreateCinemaGroupItemEndpoint()
            .GetAllCinemaGroupItemEndpoint()
            .AddCinemaHallGroupItemEndpoint();
    }
}