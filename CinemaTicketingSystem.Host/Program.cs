using CinemaTicketingSystem.API;
using CinemaTicketingSystem.API.Catalog;
using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Schedule;
using CinemaTicketingSystem.Application;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Domain;
using CinemaTicketingSystem.Host;
using CinemaTicketingSystem.Persistence;
using FluentValidation;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.RegisterLocalization();
builder.Services.RegisterCaching();
builder.Services.RegisterServiceBus(builder.Configuration);
builder.Services.RegisterDomainServices(typeof(ApplicationAssembly).Assembly);
builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.RegisterConventions(typeof(ApplicationAssembly).Assembly,
    typeof(ApplicationAbstractionAssembly).Assembly);


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

var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
//app.UseAuthentication();
//app.UseAuthorization();
app.AddCatalogGroupEndpointExt(app.AddVersionSetExt());
app.AddScheduleGroupEndpointExt(app.AddVersionSetExt());

app.Run();