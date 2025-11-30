#region

using CinemaTicketingSystem.Application.Contracts.Accounts;
using CinemaTicketingSystem.Presentation.API.Extensions;
using CinemaTicketingSystem.Presentation.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Account.SignIn;

public static class SignInEndpoint
{
    public static RouteGroupBuilder SignInGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("auth/signin",
                async (SignInRequest request,
                        [FromServices] IAccountAppService accountAppService) =>
                    (await accountAppService.SignInAsync(request)).ToGenericResult())
            .WithName("SignIn")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<SignInRequestValidator>>();


        return group;
    }
}