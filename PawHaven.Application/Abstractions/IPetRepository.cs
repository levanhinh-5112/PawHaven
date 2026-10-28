using PawHaven.Domain.Entities;

namespace PawHaven.Application.Abstractions;

/// <summary>
/// Repository abstraction for Pet persistence operations.
/// This interface will be implemented in the infrastructure layer.
/// </summary>
public interface IPetRepository
{
    /// <summary>
    /// Adds a new pet to the repository.
    /// </summary>
    /// <param name="pet">The pet to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(Pet pet, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a pet by its ID.
    /// </summary>
    /// <param name="petId">The ID of the pet to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The pet if found; otherwise null.</returns>
    Task<Pet?> GetByIdAsync(int petId, CancellationToken cancellationToken = default);
}
