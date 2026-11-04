using PawHaven.Application.Abstractions;

namespace PawHaven.Infrastructure.Security;

/// <summary>
/// BCrypt-based implementation of IPasswordHasher for secure password operations.
/// Provides methods for hashing passwords and verifying them against stored hashes.
/// </summary>
internal class PasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 11; // Balanced security/performance (100-300ms per hash)

    /// <summary>
    /// Hashes a plaintext password using bcrypt with a cost factor of 11.
    /// </summary>
    /// <param name="password">The plaintext password to hash.</param>
    /// <returns>The bcrypt hash of the password.</returns>
    /// <exception cref="ArgumentException">Thrown if password exceeds 72 bytes (bcrypt limitation).</exception>
    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

        try
        {
            // BCrypt.Net-Next handles UTF-8 encoding internally
            // Cost factor 11: ~100-200ms per hash (reasonable security/performance tradeoff)
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WorkFactor);
        }
        catch (ArgumentException ex) when (ex.Message.Contains("password"))
        {
            // Bcrypt has a 72-byte limitation for UTF-8 encoded passwords
            throw new ArgumentException("Password exceeds maximum length (72 bytes when UTF-8 encoded).", nameof(password), ex);
        }
    }

    /// <summary>
    /// Verifies a plaintext password against a bcrypt hash.
    /// </summary>
    /// <param name="password">The plaintext password to verify.</param>
    /// <param name="hash">The bcrypt hash to verify against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public bool VerifyPassword(string password, string hash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));
        ArgumentException.ThrowIfNullOrWhiteSpace(hash, nameof(hash));

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch (ArgumentException)
        {
            // Invalid hash format - return false instead of throwing
            return false;
        }
    }
}
