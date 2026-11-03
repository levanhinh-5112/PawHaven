namespace PawHaven.Application.DTOs;

/// <summary>
/// Data transfer object for authentication endpoint responses.
/// Contains the JWT access token, its expiry time, and a summary of the authenticated user.
/// Sent in responses from /auth/register and /auth/login endpoints.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// Gets the JWT access token to be included in the Authorization header.
    /// </summary>
    public string AccessToken { get; init; } = string.Empty;

    /// <summary>
    /// Gets the expiry time of the access token in UTC.
    /// Clients should discard the token after this time.
    /// </summary>
    public DateTime ExpiryTimeUtc { get; init; }

    /// <summary>
    /// Gets a summary of the authenticated user (id, email, name, role).
    /// </summary>
    public required UserSummaryDto User { get; init; }
}
