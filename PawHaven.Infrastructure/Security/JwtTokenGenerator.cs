using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PawHaven.Application.Abstractions;
using PawHaven.Domain.Entities;
using PawHaven.Infrastructure.Options;

namespace PawHaven.Infrastructure.Security;

/// <summary>
/// JWT-based implementation of IJwtTokenGenerator for token generation.
/// Generates signed JWT tokens with standard and custom claims for authenticated users.
/// Uses strongly-typed JwtSettings from dependency injection container.
/// </summary>
internal class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
    {
        ArgumentNullException.ThrowIfNull(jwtOptions, nameof(jwtOptions));
        _jwtSettings = jwtOptions.Value ?? throw new InvalidOperationException("JwtSettings configuration is invalid.");
    }

    /// <summary>
    /// Generates a signed JWT access token for a user.
    /// The token includes standard claims (sub, email) and custom claims (full_name, role).
    /// Configuration is validated at dependency injection registration time, not at token generation time.
    /// </summary>
    /// <param name="user">The user entity for which to generate a token.</param>
    /// <returns>A tuple containing the JWT token string and its UTC expiry time.</returns>
    /// <exception cref="ArgumentNullException">Thrown if user is null.</exception>
    public (string AccessToken, DateTime ExpiryTimeUtc) GenerateToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        // Create signing key (must be at least 256 bits / 32 characters for HS256)
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create claims with standard and custom data
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString(), ClaimValueTypes.String),
            new Claim(JwtRegisteredClaimNames.Email, user.Email, ClaimValueTypes.String),
            new Claim("full_name", user.FullName, ClaimValueTypes.String),
            new Claim("role", user.Role.ToString(), ClaimValueTypes.String),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        // Calculate expiry time
        var nowUtc = DateTime.UtcNow;
        var expiryTimeUtc = nowUtc.AddMinutes(_jwtSettings.ExpirationMinutes);

        // Create JWT descriptor with strongly-typed settings
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiryTimeUtc,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = credentials
        };

        // Generate and serialize token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return (tokenString, expiryTimeUtc);
    }
}
