using Microsoft.Extensions.DependencyInjection;

namespace PawHaven.Application.Extensions;

/// <summary>
/// Dependency injection extension methods for the Application layer.
/// </summary>
public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Adds Application layer services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to register services in.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Application layer services are registered here
        // Currently empty as placeholder; future handlers and services will be added here
        return services;
    }
}
