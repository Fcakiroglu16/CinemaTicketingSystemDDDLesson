using CinemaTicketingSystem.API;
using CinemaTicketingSystem.API.CinemaManagement;
using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Domain;
using CinemaTicketingSystem.Host;
using CinemaTicketingSystem.Persistence;
using FluentValidation;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();


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

builder.Services.AddMassTransit(configure =>
{

    configure.UsingInMemory();
});


builder.Services.AddVersioningExt();
var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.AddCinemaManagementGroupEndpointExt(app.AddVersionSetExt());

app.Run();