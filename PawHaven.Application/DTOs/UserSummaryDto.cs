using PawHaven.Domain.Enums;

namespace PawHaven.Application.DTOs;

/// <summary>
/// Data transfer object for user information in API responses.
/// Deliberately excludes sensitive data like password hashes.
/// Used in auth responses and user profile endpoints.
/// </summary>
public class UserSummaryDto
{
    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets the full name of the user.
    /// </summary>
    public string FullName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the role of the user (Public, Staff, or Admin).
    /// </summary>
    public UserRole Role { get; init; }
}
