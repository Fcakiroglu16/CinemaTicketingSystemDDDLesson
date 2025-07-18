using CinemaTicketingSystem.API;
using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Movie;
using CinemaTicketingSystem.Application;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Domain;
using CinemaTicketingSystem.Host;
using CinemaTicketingSystem.Persistence;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();


builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.AddWithConventions(typeof(ApplicationAssembly).Assembly,
    typeof(ApplicationAbstractionAssembly).Assembly);

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(typeof(ApplicationAssembly).Assembly, typeof(DomainAssembly).Assembly,
        typeof(PersistenceAssembly).Assembly);
});

builder.Services.AddValidatorsFromAssembly(typeof(ApiAssembly).Assembly);

builder.Services.AddVersioningExt();
var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.AddMovieGroupEndpointExt(app.AddVersionSetExt());

app.Run();