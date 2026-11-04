using PawHaven.Application.Abstractions;

namespace PawHaven.Infrastructure.Security;

/// <summary>
/// Placeholder implementation of IPasswordHasher.
/// Will be replaced with a real bcrypt-based implementation in PAW-35.
/// </summary>
internal class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        // TODO: PAW-35 - Implement bcrypt or similar hashing algorithm
        // For now, return a placeholder hash
        return $"PLACEHOLDER_HASH_{password}";
    }

    public bool VerifyPassword(string password, string hash)
    {
        // TODO: PAW-35 - Implement bcrypt or similar verification
        // For now, simple placeholder verification
        return hash == $"PLACEHOLDER_HASH_{password}";
    }
}
