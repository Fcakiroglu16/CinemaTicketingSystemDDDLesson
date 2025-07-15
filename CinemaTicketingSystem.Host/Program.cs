using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Movie;
using CinemaTicketingSystem.Application;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Host;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();


builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.AddWithConventions(typeof(ApplicationAssembly).Assembly,
    typeof(ApplicationAbstractionAssembly).Assembly);
builder.Services.AddVersioningExt();
var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();
app.AddMovieGroupEndpointExt(app.AddVersionSetExt());

app.Run();