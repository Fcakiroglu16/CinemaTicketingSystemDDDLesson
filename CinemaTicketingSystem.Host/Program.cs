#region

using CinemaTicketingSystem.Application;
using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Domain;
using CinemaTicketingSystem.Infrastructure.Authentication;
using CinemaTicketingSystem.Infrastructure.Persistence;
using CinemaTicketingSystem.Presentation.API;
using CinemaTicketingSystem.Presentation.API.Account;
using CinemaTicketingSystem.Presentation.API.Catalog;
using CinemaTicketingSystem.Presentation.API.Extensions;
using CinemaTicketingSystem.Presentation.API.Purchase;
using CinemaTicketingSystem.Presentation.API.Schedule;
using CinemaTicketingSystem.Presentation.API.Ticketing;
using CinemaTicketingSystem.WebApi.Host.Extensions;
using CinemaTicketingSystem.WebApi.Host.Identities;
using FluentValidation;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

#endregion

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();


builder.Services.RegisterLocalization();
builder.Services.RegisterCaching();
builder.Services.AddOptions(builder.Configuration);
builder.Services.RegisterServiceBus(builder.Configuration);
builder.Services.RegisterDomainServices(typeof(ApplicationAssembly).Assembly);
builder.Services.RegisterPersistence(builder.Configuration);
builder.Services.RegisterConventions(typeof(ApplicationAssembly).Assembly,
    typeof(ApplicationAbstractionAssembly).Assembly, typeof(IdentityAssembly).Assembly, typeof(ApiAssembly).Assembly);


builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(typeof(ApplicationAssembly).Assembly, typeof(DomainAssembly).Assembly,
        typeof(PersistenceAssembly).Assembly);
});


builder.Services.AddValidatorsFromAssembly(typeof(ApiAssembly).Assembly);
builder.Services.AddScoped<AppDependencyService>();
builder.Services.AddVersioningExt();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<BusinessExceptionHandler>().AddExceptionHandler<UserFriendlyExceptionHandler>()
    .AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.RegisterIdentity(builder.Configuration);
WebApplication app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


IOptions<RequestLocalizationOptions> locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
app.AddCatalogGroupEndpointExt(app.AddVersionSetExt());
app.AddScheduleGroupEndpointExt(app.AddVersionSetExt());
app.AddAccountGroupEndpointExt(app.AddVersionSetExt());
app.AddTicketingGroupEndpointExt(app.AddVersionSetExt());
app.AddPurchaseGroupEndpointExt(app.AddVersionSetExt());

app.UseAuthentication();
app.UseAuthorization();
app.Run();

public partial class Program
{
}