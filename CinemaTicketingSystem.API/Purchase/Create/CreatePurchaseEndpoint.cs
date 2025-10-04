#region

using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application.Contracts.Purchase;
using CinemaTicketingSystem.Presentation.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Purchase.Create;

public static class CreatePurchaseEndpoint
{
    public static RouteGroupBuilder CreatePurchaseGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("",
                async (CreatePurchaseRequest request,
                        [FromServices] IPurchaseAppService purcahseAppService) =>
                    (await purcahseAppService.Create(request)).ToGenericResult())
            .WithName("Create")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<CreatePurchaseValidator>>();


        return group;
    }
}