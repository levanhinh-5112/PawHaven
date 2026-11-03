namespace PawHaven.Application.Abstractions;

/// <summary>
/// Abstraction for password hashing and verification operations.
/// Implementations must never expose or log plaintext passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plaintext password using a secure hashing algorithm.
    /// </summary>
    /// <param name="password">The plaintext password to hash.</param>
    /// <returns>A hashed password string suitable for storage in the database.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a plaintext password against a stored password hash.
    /// </summary>
    /// <param name="password">The plaintext password to verify.</param>
    /// <param name="hash">The stored password hash from the database.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    bool VerifyPassword(string password, string hash);
}
