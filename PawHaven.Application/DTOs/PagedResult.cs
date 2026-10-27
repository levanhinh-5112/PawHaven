namespace PawHaven.Application.DTOs;

/// <summary>
/// A generic wrapper for paginated results.
/// Used in API responses for list endpoints with pagination.
/// </summary>
/// <typeparam name="T">The type of items in the paginated result.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Gets the items in the current page.
    /// </summary>
    public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();

    /// <summary>
    /// Gets the current page number (1-based).
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    public long TotalCount { get; init; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; init; }

    /// <summary>
    /// Gets a value indicating whether there are more pages after the current one.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>
    /// Gets a value indicating whether there are pages before the current one.
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// Creates a new PagedResult with pagination information.
    /// Automatically calculates TotalPages based on TotalCount and PageSize.
    /// </summary>
    /// <param name="items">The items in the current page.</param>
    /// <param name="page">The current page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <exception cref="ArgumentNullException">Thrown when items is null.</exception>
    /// <exception cref="ArgumentException">Thrown when page or pageSize is less than 1, or totalCount is negative.</exception>
    public PagedResult(IEnumerable<T> items, int page, int pageSize, long totalCount)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items), "Items collection cannot be null.");

        if (page < 1)
            throw new ArgumentException("Page must be 1 or greater.", nameof(page));

        if (pageSize < 1)
            throw new ArgumentException("Page size must be 1 or greater.", nameof(pageSize));

        if (totalCount < 0)
            throw new ArgumentException("Total count cannot be negative.", nameof(totalCount));

        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        // When totalCount is 0, TotalPages should be 0 (no pages for empty data)
        TotalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
