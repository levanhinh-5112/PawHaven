using PawHaven.Application.Abstractions;
using PawHaven.Domain.Entities;

namespace PawHaven.Infrastructure.Security;

/// <summary>
/// Placeholder implementation of IJwtTokenGenerator.
/// Will be replaced with a real JWT-based implementation in PAW-35.
/// </summary>
internal class JwtTokenGenerator : IJwtTokenGenerator
{
    public (string AccessToken, DateTime ExpiryTimeUtc) GenerateToken(User user)
    {
        // TODO: PAW-35 - Implement real JWT token generation using System.IdentityModel.Tokens.Jwt
        // For now, return placeholder token and expiry
        var expiryTimeUtc = DateTime.UtcNow.AddHours(1);
        var placeholderToken = $"PLACEHOLDER_TOKEN_{user.Id}_{DateTime.UtcNow.Ticks}";

        return (placeholderToken, expiryTimeUtc);
    }
}
