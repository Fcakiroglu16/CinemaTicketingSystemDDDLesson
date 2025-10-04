#region

using Asp.Versioning.Builder;
using CinemaTicketingSystem.API.Catalog.Cinema.GetAll;
using CinemaTicketingSystem.API.Catalog.Cinema.Hall;
using CinemaTicketingSystem.API.Catalog.Movie;
using CinemaTicketingSystem.Presentation.API.Catalog.Cinema.Create;
using CinemaTicketingSystem.Presentation.API.Catalog.Cinema.Hall.Add;
using CinemaTicketingSystem.Presentation.API.Catalog.Movie.Create;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

#endregion

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
            .GetAllMovieGroupItemEndpoint()
            .GetAllCinemaHallGroupItemEndpoint()
            .AddCinemaHallGroupItemEndpoint();
    }
}