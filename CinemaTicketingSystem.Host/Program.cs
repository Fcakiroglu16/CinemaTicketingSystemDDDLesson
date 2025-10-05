#region

using CinemaTicketingSystem.API;
using CinemaTicketingSystem.API.Account;
using CinemaTicketingSystem.API.Catalog;
using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Schedule;
using CinemaTicketingSystem.Application;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Domain;
using CinemaTicketingSystem.Host.Identities;
using CinemaTicketingSystem.Identity;
using CinemaTicketingSystem.Persistence;
using CinemaTicketingSystem.Presentation.API.Purchase;
using CinemaTicketingSystem.Presentation.API.Ticketing;
using CinemaTicketingSystem.WebApi.Host.Extensions;
using FluentValidation;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

#endregion

var builder = WebApplication.CreateBuilder(args);


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
var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
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