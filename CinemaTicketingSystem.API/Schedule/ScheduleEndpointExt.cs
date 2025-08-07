#region

using Asp.Versioning.Builder;
using CinemaTicketingSystem.API.Schedule.GetAll;
using CinemaTicketingSystem.Presentation.API.Schedule.AddMovieToHall;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

#endregion

namespace CinemaTicketingSystem.API.Schedule;

public static class ScheduleEndpointExt
{
    public static void AddScheduleGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/schedules").WithTags("schedules")
            .WithApiVersionSet(apiVersionSet).AddMovieToHallGroupItemEndpoint().GetMoviesByHallIdGroupItemEndpoint();
    }
}