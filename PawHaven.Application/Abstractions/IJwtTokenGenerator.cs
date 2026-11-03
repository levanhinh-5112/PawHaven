using PawHaven.Domain.Entities;

namespace PawHaven.Application.Abstractions;

/// <summary>
/// Abstraction for JWT token generation.
/// Generates tokens containing user claims (id, email, name, role) with configurable expiry.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a JWT access token for the given user.
    /// </summary>
    /// <param name="user">The user entity to generate a token for.</param>
    /// <returns>A tuple containing the access token and its expiry time in UTC.</returns>
    (string AccessToken, DateTime ExpiryTimeUtc) GenerateToken(User user);
}
