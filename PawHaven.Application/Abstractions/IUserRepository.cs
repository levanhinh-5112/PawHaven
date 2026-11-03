using PawHaven.Domain.Entities;

namespace PawHaven.Application.Abstractions;

/// <summary>
/// Abstraction for user data access operations.
/// Implementations manage user persistence, retrieval, and email uniqueness checks.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Finds a user by their email address.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The User if found; otherwise, null.</returns>
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a user by their unique identifier.
    /// </summary>
    /// <param name="id">The user ID to search for.</param>
    /// <returns>The User if found; otherwise, null.</returns>
    Task<User?> FindByIdAsync(int id);

    /// <summary>
    /// Checks whether an email address is already registered.
    /// </summary>
    /// <param name="email">The email to check.</param>
    /// <returns>True if the email exists; otherwise, false.</returns>
    Task<bool> EmailExistsAsync(string email);

    /// <summary>
    /// Adds a new user to the database.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(User user);
}
