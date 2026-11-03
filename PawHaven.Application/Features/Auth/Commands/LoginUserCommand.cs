namespace PawHaven.Application.Features.Auth.Commands;

/// <summary>
/// Command to authenticate a user and generate a JWT token.
/// </summary>
public record LoginUserCommand(
    string Email,
    string Password);
