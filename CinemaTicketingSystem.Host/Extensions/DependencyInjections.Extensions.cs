#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers;
using CinemaTicketingSystem.Application.Ticketing.EventHandlers;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.IntegrationEvents;
using CinemaTicketingSystem.Domain.BoundedContexts.Purchases.DomainEvents;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Infrastructure.Caching;
using CinemaTicketingSystem.Infrastructure.Messaging;
using CinemaTicketingSystem.Infrastructure.Persistence;
using CinemaTicketingSystem.Infrastructure.Persistence.Accounts;
using CinemaTicketingSystem.Presentation.API.Localization;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Options;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Reflection;

#endregion

namespace CinemaTicketingSystem.WebApi.Host.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<TokenOption>(configuration.GetSection(nameof(TokenOption)));
        services.Configure<ClientOption>(configuration.GetSection(nameof(ClientOption)));


        services.AddSingleton(sp => sp.GetRequiredService<IOptions<TokenOption>>().Value);

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<ClientOption>>().Value);
        return services;
    }


    public static IServiceCollection RegisterServiceBus(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddScoped<IIntegrationEventHandler<CinemaHallCreatedIntegrationEvent>,
                CinemaHallCreatedIntegrationEventHandler>();
        services
            .AddScoped<IIntegrationEventHandler<MovieCreatedIntegrationEvent>, MovieCreatedIntegrationEventHandler>();

        services
            .AddScoped<IIntegrationEventHandler<PurchaseCreatedIntegrationEvent>,
                PurchaseCreatedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventBus, IntegrationEventBus>();
        services.AddScoped<IDomainEventMediator, DomainEventMediator>();
        services.AddMassTransit(configure =>
        {
            configure.AddConsumer<MassTransitConsumerAdapter<CinemaHallCreatedIntegrationEvent>>();
            configure.AddConsumer<MassTransitConsumerAdapter<MovieCreatedIntegrationEvent>>();
            configure.AddConsumer<MassTransitConsumerAdapter<PurchaseCreatedIntegrationEvent>>();

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
            string[] supportedCultures = new[] { "en", "tr" };
            options.SetDefaultCulture("en");
            options.AddSupportedCultures(supportedCultures);
            options.AddSupportedUICultures(supportedCultures);
        });
        services.AddScoped<ILocalizer, Localizer>();
        return services;
    }


    public static IServiceCollection RegisterPersistence(this IServiceCollection services,
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
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<AppDbContext>();


        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.RegisterRepositories();


        return services;
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        Assembly persistenceAssembly = typeof(PersistenceAssembly).Assembly;

        List<Type> repositoryTypes = persistenceAssembly
            .GetTypes()
            .Where(type => type.Name.EndsWith("Repository") &&
                           type.IsClass &&
                           !type.IsAbstract &&
                           !type.IsGenericTypeDefinition)
            .ToList();

        foreach (Type? repositoryType in repositoryTypes)
        {
            List<Type> interfaces = repositoryType.GetInterfaces()
                .Where(i => i.Name.EndsWith("Repository") && i != typeof(IGenericRepository<>))
                .ToList();

            if (interfaces.Any())
                foreach (Type? interfaceType in interfaces)
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
        List<Type> domainServiceTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false } &&
                           typeof(IDomainService).IsAssignableFrom(type))
            .ToList();

        foreach (Type? domainServiceType in domainServiceTypes) services.AddTransient(domainServiceType);

        return services;
    }


    public static IServiceCollection RegisterConventions(this IServiceCollection services, params Assembly[] assemblies)
    {
        IEnumerable<TypeInfo> allTypes = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(t => t is { IsClass: true, IsAbstract: false, IsPublic: true });

        foreach (TypeInfo? implementationType in allTypes)
        {
            List<Type> serviceTypes = implementationType.ImplementedInterfaces
                .Where(i => i != typeof(ITransientDependency) &&
                            i != typeof(IScopedDependency) &&
                            i != typeof(ISingletonDependency))
                .ToList();

            ServiceLifetime? lifetime = GetLifetime(implementationType);

            if (lifetime == null)
                continue;


            if (!serviceTypes.Any())
            {
                services.Add(new ServiceDescriptor(implementationType, implementationType, lifetime.Value));
                continue;
            }

            foreach (Type? serviceType in serviceTypes)
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