using PawHaven.Application.Abstractions;
using PawHaven.Domain.Entities;
using PawHaven.Infrastructure.Persistence;

namespace PawHaven.Infrastructure.Repositories;

/// <summary>
/// Placeholder implementation of IUserRepository.
/// Will be replaced with a real database-backed implementation in PAW-35.
/// </summary>
internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        // TODO: PAW-35 - Implement database query
        return await Task.FromResult<User?>(null);
    }

    public async Task<User?> FindByIdAsync(int id)
    {
        // TODO: PAW-35 - Implement database query
        return await Task.FromResult<User?>(null);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        // TODO: PAW-35 - Implement database query
        return await Task.FromResult(false);
    }

    public async Task AddAsync(User user)
    {
        // TODO: PAW-35 - Implement database insert
        await Task.CompletedTask;
    }
}
