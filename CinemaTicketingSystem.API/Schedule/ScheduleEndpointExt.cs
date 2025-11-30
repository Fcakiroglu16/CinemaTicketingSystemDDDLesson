#region

using Asp.Versioning.Builder;
using CinemaTicketingSystem.Presentation.API.Schedule.AddMovieToHall;
using CinemaTicketingSystem.Presentation.API.Schedule.GetAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Schedule;

public static class ScheduleEndpointExt
{
    public static void AddScheduleGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/schedules").WithTags("schedules")
            .WithApiVersionSet(apiVersionSet).AddMovieToHallGroupItemEndpoint().GetMoviesByHallIdGroupItemEndpoint();
    }
}