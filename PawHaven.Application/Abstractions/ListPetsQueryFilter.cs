using PawHaven.Application.Features.Pets.Queries;
using PawHaven.Domain.Enums;

namespace PawHaven.Application.Abstractions;

/// <summary>
/// Filter object for list pet queries, encapsulating optional filtering and sorting logic.
/// Used to decouple query parameters from the repository interface.
/// </summary>
public class ListPetsQueryFilter
{
    /// <summary>
    /// Gets the optional pet status filter.
    /// If null, no status filtering is applied.
    /// </summary>
    public PetStatus? Status { get; init; }

    /// <summary>
    /// Gets the optional pet species filter.
    /// If null, no species filtering is applied.
    /// </summary>
    public Species? Species { get; init; }

    /// <summary>
    /// Gets the sort field name (e.g., "name", "age", "created").
    /// If null or empty, defaults to "created".
    /// </summary>
    public string? SortBy { get; init; }

    /// <summary>
    /// Gets the sort direction ("asc" or "desc").
    /// If null or empty, defaults to "desc".
    /// </summary>
    public string? SortDirection { get; init; }

    /// <summary>
    /// Creates a new ListPetsQueryFilter from a ListPetsQuery.
    /// </summary>
    /// <param name="query">The query to extract filter parameters from.</param>
    /// <returns>A new ListPetsQueryFilter with parameters from the query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when query is null.</exception>
    public static ListPetsQueryFilter FromQuery(ListPetsQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return new ListPetsQueryFilter
        {
            Status = query.Status,
            Species = query.Species,
            SortBy = query.SortBy,
            SortDirection = query.SortDirection
        };
    }
}
