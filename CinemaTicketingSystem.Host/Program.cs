using CinemaTicketingSystem.API;
using CinemaTicketingSystem.API.Catalog;
using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Localization;
using CinemaTicketingSystem.API.Schedule;
using CinemaTicketingSystem.Application;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers;
using CinemaTicketingSystem.Domain;
using CinemaTicketingSystem.Domain.Catalog.DomainEvents;
using CinemaTicketingSystem.Host;
using CinemaTicketingSystem.Persistence;
using CinemaTicketingSystem.ServiceBus;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "tr" };
    options.SetDefaultCulture("en");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.AddWithConventions(typeof(ApplicationAssembly).Assembly,
    typeof(ApplicationAbstractionAssembly).Assembly);


builder.Services.RegisterDomainServices(typeof(ApplicationAssembly).Assembly);


builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(typeof(ApplicationAssembly).Assembly, typeof(DomainAssembly).Assembly,
        typeof(PersistenceAssembly).Assembly);
});


builder.Services.AddValidatorsFromAssembly(typeof(ApiAssembly).Assembly);


builder.Services.AddScoped<IEventHandler<CinemaHallCreatedEvent>, CinemaHallCreatedEventHandler>();
builder.Services.AddScoped<IEventHandler<MovieCreatedEvent>, MovieCreatedEventHandler>();

builder.Services.AddScoped<IIntegrationEventBus, IntegrationEventBus>();

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<MassTransitConsumerAdapter<CinemaHallCreatedEvent>>();
    configure.AddConsumer<MassTransitConsumerAdapter<MovieCreatedEvent>>();
    configure.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
});


builder.Services.AddVersioningExt();






var app = builder.Build();

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.AddCatalogGroupEndpointExt(app.AddVersionSetExt());
app.AddScheduleGroupEndpointExt(app.AddVersionSetExt());

app.Run();