using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PawHaven.Application.Abstractions.Handlers;

namespace PawHaven.Application.Extensions;

/// <summary>
/// Dependency injection extension methods for the Application layer.
/// </summary>
public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Adds Application layer services to the dependency injection container.
    /// Automatically registers all validators and CQRS handlers via assembly scanning.
    /// </summary>
    /// <param name="services">The service collection to register services in.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationServiceExtensions).Assembly;

        // Register all validators from the Application assembly
        services.AddValidatorsFromAssembly(assembly);

        // Register all command handlers (ICommandHandler<,>)
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        // Register all query handlers (IQueryHandler<,>)
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}
