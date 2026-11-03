namespace PawHaven.Application.Exceptions;

/// <summary>
/// Thrown when user authentication fails (wrong email or password).
/// This exception is caught by the exception handling middleware and mapped to a 401 Unauthorized response.
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
