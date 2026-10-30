using PawHaven.Application.Abstractions;
using PawHaven.Infrastructure.Persistence;

namespace PawHaven.Infrastructure.UnitOfWork;

/// <summary>
/// Unit of Work implementation using Entity Framework Core DbContext.
/// Coordinates repository operations and persists changes to the database.
/// </summary>
internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Saves all pending changes to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
