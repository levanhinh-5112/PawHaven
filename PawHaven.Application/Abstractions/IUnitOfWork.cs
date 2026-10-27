namespace PawHaven.Application.Abstractions;

/// <summary>
/// Unit of Work abstraction for coordinating repository operations and persisting changes.
/// Repositories are responsible for querying and adding/removing entities.
/// The unit of work is responsible for persisting those changes to the database.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all pending changes to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
