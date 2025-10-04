#region

using CinemaTicketingSystem.API.Account.SignUp;
using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application.Abstraction.Accounts;
using CinemaTicketingSystem.Application.Contracts.Accounts;
using CinemaTicketingSystem.Presentation.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Account.SignUp;

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