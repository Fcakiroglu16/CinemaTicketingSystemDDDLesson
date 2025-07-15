using Asp.Versioning.Builder;
using CinemaTicketingSystem.API.Movie.Create;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


namespace CinemaTicketingSystem.API.Movie
{
    public static class MovieEndpointExt
    {
        public static void AddMovieGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            app.MapGroup("api/v{version:apiVersion}/movies").WithTags("movies")
                .WithApiVersionSet(apiVersionSet)
                .CreateMovieGroupItemEndpoint();
        }
    }
}