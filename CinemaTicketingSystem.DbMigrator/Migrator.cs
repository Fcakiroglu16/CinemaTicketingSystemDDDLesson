using CinemaTicketingSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketingSystem.DbMigrator
{
    public class Migrator(ILogger<Migrator> logger, IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {


            logger.LogInformation("Migrator running.");

            using var scope = serviceProvider.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            appDbContext.Database.MigrateAsync(cancellationToken: stoppingToken);


            hostApplicationLifetime.StopApplication();
            return Task.CompletedTask;
        }
    }
}
