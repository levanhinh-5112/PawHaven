using PawHaven.Application.Abstractions;
using PawHaven.Application.Abstractions.Handlers;
using PawHaven.Application.DTOs;
using PawHaven.Application.Exceptions;
using PawHaven.Domain.Entities;

namespace PawHaven.Application.Features.Auth.Commands;

/// <summary>
/// Handler for the RegisterUserCommand.
/// Coordinates user registration by validating uniqueness, hashing password, creating user entity,
/// persisting to repository, and generating JWT token.
/// </summary>
public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<AuthResponseDto> HandleAsync(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Check if email already exists
        var emailExists = await _userRepository.EmailExistsAsync(command.Email);
        if (emailExists)
        {
            throw new ConflictException("User", command.Email, $"Email '{command.Email}' is already registered.");
        }

        // Hash the password
        var passwordHash = _passwordHasher.HashPassword(command.Password);

        // Create User entity with validation (ArgumentException thrown on invalid input)
        User user;
        try
        {
            user = new User(
                email: command.Email,
                passwordHash: passwordHash,
                fullName: command.FullName);
        }
        catch (ArgumentException ex)
        {
            // Convert domain validation exception to application exception
            throw ValidationException.FromArgumentException(ex);
        }

        // Persist user to repository
        await _userRepository.AddAsync(user);

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
