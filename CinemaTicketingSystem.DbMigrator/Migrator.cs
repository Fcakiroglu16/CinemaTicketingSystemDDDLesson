namespace CinemaTicketingSystem.DbMigrator
{
    public class Migrator(ILogger<Migrator> logger) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {


            logger.LogInformation("Migrator running.");
            return Task.CompletedTask;




        }
    }
}
