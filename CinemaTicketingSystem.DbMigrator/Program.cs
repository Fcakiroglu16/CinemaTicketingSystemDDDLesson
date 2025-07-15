using CinemaTicketingSystem.DbMigrator;
using CinemaTicketingSystem.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<Migrator>();

var host = builder.Build();
host.Run();
