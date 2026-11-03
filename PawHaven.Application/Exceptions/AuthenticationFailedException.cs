namespace PawHaven.Application.Exceptions;

/// <summary>
/// Thrown when authentication fails (e.g., invalid credentials).
/// Intended to be translated by the presentation layer into an appropriate error response.
/// </summary>
public class AuthenticationFailedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the AuthenticationFailedException class.
    /// </summary>
    /// <param name="message">The exception message (default: "Authentication failed").</param>
    public AuthenticationFailedException(string message = "Authentication failed")
        : base(message)
    {
    }
}
