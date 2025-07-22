using Asp.Versioning.Builder;
using CinemaTicketingSystem.API.Schedule.AddMovieToHall;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CinemaTicketingSystem.API.Schedule;

public static class ScheduleEndpointExt
{
    public static void AddScheduleGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/schedules").WithTags("schedules")
            .WithApiVersionSet(apiVersionSet).AddMovieToHallGroupItemEndpoint().GetMoviesByHallIdGroupItemEndpoint();
    }
}