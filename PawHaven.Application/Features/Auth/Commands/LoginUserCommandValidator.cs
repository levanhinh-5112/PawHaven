using FluentValidation;

namespace PawHaven.Application.Features.Auth.Commands;

/// <summary>
/// Validator for LoginUserCommand.
/// Enforces that email and password are provided.
/// </summary>
public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}
