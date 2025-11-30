#region

using CinemaTicketingSystem.Infrastructure.DbMigrator;
using CinemaTicketingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

#endregion

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CinemaTicketingDb")));
builder.Services.AddHostedService<Migrator>();

IHost host = builder.Build();
host.Run();