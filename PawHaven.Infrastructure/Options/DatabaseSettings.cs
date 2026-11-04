using System.ComponentModel.DataAnnotations;

namespace PawHaven.Infrastructure.Options;

/// <summary>
/// Configuration settings for database connections.
/// Maps to the "ConnectionStrings" section in appsettings.json and User Secrets.
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Gets or sets the default database connection string.
    /// Used to connect to the primary PawHaven database (SQL Server).
    /// Format: Server=hostname;Database=dbname;User Id=username;Password=password;...
    /// </summary>
    [Required(ErrorMessage = "Database connection string is required. Set 'ConnectionStrings:DefaultConnection' in configuration, User Secrets, or environment variables.")]
    [MinLength(10, ErrorMessage = "Connection string appears to be invalid.")]
    public string DefaultConnection { get; set; } = string.Empty;
}
