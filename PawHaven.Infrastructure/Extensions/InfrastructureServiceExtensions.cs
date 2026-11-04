using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PawHaven.Application.Abstractions;
using PawHaven.Infrastructure.Options;
using PawHaven.Infrastructure.Persistence;
using PawHaven.Infrastructure.Repositories;
using PawHaven.Infrastructure.Security;
using PawHaven.Infrastructure.UnitOfWork;

namespace PawHaven.Infrastructure.Extensions;

/// <summary>
/// Dependency injection extension methods for the Infrastructure layer.
/// </summary>
public static class InfrastructureServiceExtensions
{
    private const string DefaultConnectionName = "DefaultConnection";

    /// <summary>
    /// Adds Infrastructure layer services to the dependency injection container.
    /// Registers configuration options, database context, repositories, and authentication services.
    /// </summary>
    /// <param name="services">The service collection to register services in.</param>
    /// <param name="configuration">The application configuration used to resolve settings and connection strings.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register strongly-typed configuration options with validation
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection("Jwt"))
            .ValidateDataAnnotations();

        services.AddOptions<DatabaseSettings>()
            .Bind(configuration.GetSection("ConnectionStrings"))
            .ValidateDataAnnotations();

        // Get connection string from configuration for DbContext setup
        var connectionString = configuration.GetConnectionString(DefaultConnectionName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Missing required connection string 'ConnectionStrings:{DefaultConnectionName}'. Configure it in appsettings, user secrets, or environment variables.");
        }

        // Register DbContext with SQL Server provider
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Pet-related repositories
        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Auth-related services (PAW-34 handlers require these)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
