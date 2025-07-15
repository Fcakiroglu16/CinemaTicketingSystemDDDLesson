using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Persistence;
using CinemaTicketingSystem.Persistence.Accounts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CinemaTicketingSystem.Host;

public static class ServiceCollectionExtensions
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



        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<AppDbContext>();


        return services;
    }









    public static IServiceCollection AddWithConventions(this IServiceCollection services, params Assembly[] assemblies)
    {
        var allTypes = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(t => t is { IsClass: true, IsAbstract: false, IsPublic: true });

        foreach (var implementationType in allTypes)
        {
            var serviceTypes = implementationType.ImplementedInterfaces
                .Where(i => i != typeof(ITransientDependency) &&
                            i != typeof(IScopedDependency) &&
                            i != typeof(ISingletonDependency))
                .ToList();

            var lifetime = GetLifetime(implementationType);

            if (lifetime == null)
                continue;


            if (!serviceTypes.Any())
            {
                services.Add(new ServiceDescriptor(implementationType, implementationType, lifetime.Value));
                continue;
            }

            foreach (var serviceType in serviceTypes)
                services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime.Value));
        }

        return services;
    }

    private static ServiceLifetime? GetLifetime(TypeInfo type)
    {
        if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type)) return ServiceLifetime.Transient;
        if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type)) return ServiceLifetime.Scoped;
        if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type)) return ServiceLifetime.Singleton;

        return null;
    }
}