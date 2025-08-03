using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Filters;
using CinemaTicketingSystem.Application.Abstraction.Accounts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CinemaTicketingSystem.API.Account.SignUp;

public static class SignUpEndpoint
{
    public static RouteGroupBuilder SignUpGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("auth/signup",
                async (SignUpRequest request,
                        [FromServices] IAccountAppService accountAppService) =>
                    (await accountAppService.SignUpAsync(request)).ToGenericResult())
            .WithName("SignUp")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<SignUpRequestValidator>>();


        return group;
    }
}