using System.ComponentModel.DataAnnotations;

namespace PawHaven.Infrastructure.Options;

/// <summary>
/// Configuration settings for JWT token generation.
/// Maps to the "Jwt" section in appsettings.json and User Secrets.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Gets or sets the JWT signing secret key.
    /// Must be at least 32 characters long for HS256 algorithm.
    /// Stored in User Secrets during development, environment variables in production.
    /// </summary>
    [Required(ErrorMessage = "JWT Secret is required. Set 'Jwt:Secret' in User Secrets or configuration.")]
    [MinLength(32, ErrorMessage = "JWT Secret must be at least 32 characters long for HS256.")]
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token issuer identifier.
    /// Included in JWT 'iss' claim.
    /// Example: "PawHaven"
    /// </summary>
    [Required(ErrorMessage = "JWT Issuer is required.")]
    [MinLength(1, ErrorMessage = "JWT Issuer cannot be empty.")]
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the intended audience for the token.
    /// Included in JWT 'aud' claim.
    /// Example: "PawHavenClient"
    /// </summary>
    [Required(ErrorMessage = "JWT Audience is required.")]
    [MinLength(1, ErrorMessage = "JWT Audience cannot be empty.")]
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token expiration time in minutes.
    /// Default: 60 minutes (1 hour).
    /// </summary>
    [Range(1, 525600, ErrorMessage = "JWT ExpirationMinutes must be between 1 and 525600 (1 year).")]
    public int ExpirationMinutes { get; set; } = 60;
}
