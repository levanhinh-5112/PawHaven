using PawHaven.Application.DTOs;
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

    /// <summary>
    /// Retrieves a tracked pet by its ID for update operations.
    /// </summary>
    /// <param name="petId">The ID of the pet to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The pet if found; otherwise null.</returns>
    Task<Pet?> GetByIdForUpdateAsync(int petId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of pets with optional filtering and sorting.
    /// </summary>
    /// <param name="filter">The filter object containing optional status/species filters and sort parameters.</param>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A PagedResult containing pets and pagination metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown when filter is null.</exception>
    Task<PagedResult<Pet>> ListAsync(ListPetsQueryFilter filter, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a pet by its ID.
    /// </summary>
    /// <param name="petId">The ID of the pet to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<bool> DeleteAsync(int petId, CancellationToken cancellationToken = default);
}
