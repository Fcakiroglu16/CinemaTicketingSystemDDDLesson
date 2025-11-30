#region

using CinemaTicketingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CinemaTicketingSystem.Infrastructure.DbMigrator;

public class Migrator(
    ILogger<Migrator> logger,
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Migrator running.");

        using IServiceScope scope = serviceProvider.CreateScope();
        AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        appDbContext.Database.MigrateAsync(stoppingToken);


        hostApplicationLifetime.StopApplication();
        return Task.CompletedTask;
    }
}