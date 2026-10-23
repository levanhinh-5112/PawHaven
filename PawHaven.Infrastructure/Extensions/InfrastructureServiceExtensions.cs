using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PawHaven.Application.Abstractions;
using PawHaven.Infrastructure.Persistence;
using PawHaven.Infrastructure.Repositories;

namespace PawHaven.Infrastructure.Extensions;

/// <summary>
/// Dependency injection extension methods for the Infrastructure layer.
/// </summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Adds Infrastructure layer services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to register services in.</param>
    /// <param name="configuration">The application configuration used to resolve the database connection string.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found. Set it in appsettings or user secrets.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IPetRepository, PetRepository>();
        return services;
    }
}
