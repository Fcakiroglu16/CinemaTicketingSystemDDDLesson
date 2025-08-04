using CinemaTicketingSystem.Identity;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CinemaTicketingSystem.Host.Identities;

public static class IdentityExt
{
    public static IServiceCollection RegisterIdentity(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IUserContext, UserContext>();

        services.Configure<List<ClientOption>>(configuration.GetSection("Clients"));


        services.AddSingleton(sp =>
        {
            var clientTokenOption = sp.GetRequiredService<IOptions<List<ClientOption>>>();

            return clientTokenOption.Value;
        });


        services.Configure<ClientTokenOption>(configuration.GetSection(nameof(ClientTokenOption)));
        services.AddSingleton(sp =>
        {
            var clientTokenOption = sp.GetRequiredService<IOptions<ClientTokenOption>>();

            return clientTokenOption.Value;
        });


        services.Configure<TokenOption>(configuration.GetSection(nameof(TokenOption)));
        services.AddSingleton(sp =>
        {
            var clientTokenOption = sp.GetRequiredService<IOptions<TokenOption>>();

            return clientTokenOption.Value;
        });


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer("ClientCredentialSchema", opts =>
        {
            var tokenOptions = configuration.GetSection(nameof(ClientTokenOption)).Get<ClientTokenOption>();
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = tokenOptions!.Issuer,
                IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ClockSkew = TimeSpan.Zero
            };
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
        {
            var tokenOptions = configuration.GetSection(nameof(TokenOption)).Get<TokenOption>();
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = tokenOptions!.Issuer,
                IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ClockSkew = TimeSpan.Zero
            };
        });
        services.AddAuthorization(options =>
        {
            //options.AddPolicy("ClientCredential", policy =>
            //{
            //    policy.AuthenticationSchemes.Add("ClientCredentialSchema"); // "Bearer"
            //    policy.RequireAuthenticatedUser();
            //});






        });
        return services;
    }
}