namespace PawHaven.Application.Features.Auth.Commands;

/// <summary>
/// Command to register a new user account.
/// </summary>
public record RegisterUserCommand(
    string Email,
    string Password,
    string FullName);
