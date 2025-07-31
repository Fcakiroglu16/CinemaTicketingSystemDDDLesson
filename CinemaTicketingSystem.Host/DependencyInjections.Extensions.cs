using System.Reflection;
using CinemaTicketingSystem.API.Localization;
using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers;
using CinemaTicketingSystem.Caching;
using CinemaTicketingSystem.Domain.Catalog.DomainEvents;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Persistence;
using CinemaTicketingSystem.Persistence.Accounts;
using CinemaTicketingSystem.ServiceBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CinemaTicketingSystem.Host;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServiceBus(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IEventHandler<CinemaHallCreatedEvent>, CinemaHallCreatedEventHandler>();
        services.AddScoped<IEventHandler<MovieCreatedEvent>, MovieCreatedEventHandler>();

        services.AddScoped<IIntegrationEventBus, IntegrationEventBus>();

        services.AddMassTransit(configure =>
        {
            configure.AddConsumer<MassTransitConsumerAdapter<CinemaHallCreatedEvent>>();
            configure.AddConsumer<MassTransitConsumerAdapter<MovieCreatedEvent>>();
            configure.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
        });

        return services;
    }

    public static IServiceCollection RegisterCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
        return services;
    }


    public static IServiceCollection RegisterLocalization(this IServiceCollection services)
    {
        services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { "en", "tr" };
            options.SetDefaultCulture("en");
            options.AddSupportedCultures(supportedCultures);
            options.AddSupportedUICultures(supportedCultures);
        });
        services.AddScoped<ILocalizer, Localizer>();
        return services;
    }


    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("CinemaTicketingDb"),
                sqlServerDbContextOptionsBuilder =>
                {
                    sqlServerDbContextOptionsBuilder.MigrationsAssembly(typeof(PersistenceAssembly).Assembly);
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


        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.RegisterRepositories();


        return services;
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        var persistenceAssembly = typeof(PersistenceAssembly).Assembly;

        var repositoryTypes = persistenceAssembly
            .GetTypes()
            .Where(type => type.Name.EndsWith("Repository") &&
                           type.IsClass &&
                           !type.IsAbstract &&
                           !type.IsGenericTypeDefinition)
            .ToList();

        foreach (var repositoryType in repositoryTypes)
        {
            var interfaces = repositoryType.GetInterfaces()
                .Where(i => i.Name.EndsWith("Repository") && i != typeof(IGenericRepository<,>))
                .ToList();

            if (interfaces.Any())
                foreach (var interfaceType in interfaces)
                    services.AddScoped(interfaceType, repositoryType);
            else
                // Eğer interface yoksa concrete type'ı kendisi olarak kaydet
                services.AddScoped(repositoryType);
        }

        return services;
    }


    public static IServiceCollection RegisterDomainServices(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var domainServiceTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false } &&
                           typeof(IDomainService).IsAssignableFrom(type))
            .ToList();

        foreach (var domainServiceType in domainServiceTypes) services.AddTransient(domainServiceType);

        return services;
    }


    public static IServiceCollection RegisterConventions(this IServiceCollection services, params Assembly[] assemblies)
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