using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketingSystem.Persistence
{
    public static class ServiceRegister
    {
        public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CinemaTicketingDb"), options =>
                {
                    options.MigrationsAssembly(typeof(PersistenceAssembly).Assembly);
                });
            });

            return services;
        }
    }
}
