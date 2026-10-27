namespace PawHaven.Application.Abstractions.Handlers;

/// <summary>
/// Generic interface for command handlers that perform writes/mutations.
/// </summary>
/// <typeparam name="TCommand">The command type containing request data.</typeparam>
public interface ICommandHandler<in TCommand>
{
    /// <summary>
    /// Handles the command asynchronously.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Generic interface for command handlers that perform writes/mutations and return a result.
/// </summary>
/// <typeparam name="TCommand">The command type containing request data.</typeparam>
/// <typeparam name="TResult">The result type returned by the handler.</typeparam>
public interface ICommandHandler<in TCommand, TResult>
{
    /// <summary>
    /// Handles the command asynchronously and returns a result.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation with the result.</returns>
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
