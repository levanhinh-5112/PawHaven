using PawHaven.Domain.Enums;

namespace PawHaven.Application.Features.Pets.Queries;

/// <summary>
/// Query to retrieve a paginated list of pets with optional filtering and sorting.
/// </summary>
/// <param name="Page">The page number (1-based). Must be >= 1. Defaults to 1.</param>
/// <param name="PageSize">The number of items per page. Must be between 1 and 50. Defaults to 10.</param>
/// <param name="Status">Optional filter by pet status.</param>
/// <param name="Species">Optional filter by pet species.</param>
/// <param name="SortBy">Optional sort field: "name", "species", "age", "status", "created". Defaults to "created".</param>
/// <param name="SortDirection">Optional sort direction: "asc" or "desc". Defaults to "desc".</param>
public record ListPetsQuery(
    int Page = 1,
    int PageSize = 10,
    PetStatus? Status = null,
    Species? Species = null,
    string? SortBy = "created",
    string? SortDirection = "desc")
{
    /// <summary>
    /// Validates the query parameters.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    public void Validate()
    {
        if (Page < 1)
        {
            throw new ArgumentException("Page must be >= 1.", nameof(Page));
        }

        if (PageSize < 1 || PageSize > 50)
        {
            throw new ArgumentException("PageSize must be between 1 and 50.", nameof(PageSize));
        }

        var validSortFields = new[] { "name", "species", "age", "status", "created" };
        if (!string.IsNullOrEmpty(SortBy) && !validSortFields.Contains(SortBy.ToLower()))
        {
            throw new ArgumentException(
                $"SortBy must be one of: {string.Join(", ", validSortFields)}. Got: {SortBy}",
                nameof(SortBy));
        }

        var validDirections = new[] { "asc", "desc" };
        if (!string.IsNullOrEmpty(SortDirection) && !validDirections.Contains(SortDirection.ToLower()))
        {
            throw new ArgumentException(
                $"SortDirection must be 'asc' or 'desc'. Got: {SortDirection}",
                nameof(SortDirection));
        }
    }
}
