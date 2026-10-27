namespace PawHaven.Application.Abstractions.Handlers;

/// <summary>
/// Generic interface for query handlers that perform reads.
/// </summary>
/// <typeparam name="TQuery">The query type containing request parameters.</typeparam>
/// <typeparam name="TResult">The result type returned by the handler.</typeparam>
public interface IQueryHandler<in TQuery, TResult>
{
    /// <summary>
    /// Handles the query asynchronously and returns a result.
    /// </summary>
    /// <param name="query">The query to handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation with the result.</returns>
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
