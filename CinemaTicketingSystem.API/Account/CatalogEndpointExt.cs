using Asp.Versioning.Builder;
using CinemaTicketingSystem.API.Account.SignIn;
using CinemaTicketingSystem.API.Account.SignUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CinemaTicketingSystem.API.Account;

public static class AccountEndpointExt
{
    public static void AddAccountGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/accounts").WithTags("accounts")
            .WithApiVersionSet(apiVersionSet)
            .SignInGroupItemEndpoint().SignUpGroupItemEndpoint();

    }
}