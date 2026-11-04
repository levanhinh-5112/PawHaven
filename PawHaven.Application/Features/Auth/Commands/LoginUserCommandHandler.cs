using PawHaven.Application.Abstractions;
using PawHaven.Application.Abstractions.Handlers;
using PawHaven.Application.DTOs;
using PawHaven.Application.Exceptions;

namespace PawHaven.Application.Features.Auth.Commands;

/// <summary>
/// Handler for the LoginUserCommand.
/// Coordinates user authentication by finding user, verifying password,
/// and generating JWT token.
/// </summary>
public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
    }

    public async Task<AuthResponseDto> HandleAsync(LoginUserCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Find user by email
        var user = await _userRepository.FindByEmailAsync(command.Email, cancellationToken);
        if (user is null)
        {
            throw new AuthenticationFailedException("Invalid email or password.");
        }

        // Verify password
        var isPasswordValid = _passwordHasher.VerifyPassword(command.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new AuthenticationFailedException("Invalid email or password.");
        }

        // Generate JWT token
        var (accessToken, expiryTimeUtc) = _jwtTokenGenerator.GenerateToken(user);

        // Return auth response with user summary
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            ExpiryTimeUtc = expiryTimeUtc,
            User = new UserSummaryDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            }
        };
    }
}
