using Microsoft.EntityFrameworkCore;
using PawHaven.Application.Abstractions;
using PawHaven.Domain.Entities;
using PawHaven.Infrastructure.Persistence;

namespace PawHaven.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IUserRepository for user data access operations.
/// Provides methods for querying and persisting User entities to the database.
/// </summary>
internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Finds a user by their email address asynchronously.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The User if found; otherwise, null.</returns>
    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// Finds a user by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The user ID to search for.</param>
    /// <returns>The User if found; otherwise, null.</returns>
    public async Task<User?> FindByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Checks whether an email address is already registered in the database.
    /// </summary>
    /// <param name="email">The email to check.</param>
    /// <returns>True if the email exists; otherwise, false.</returns>
    public async Task<bool> EmailExistsAsync(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    /// <summary>
    /// Adds a new user to the database and saves changes.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
