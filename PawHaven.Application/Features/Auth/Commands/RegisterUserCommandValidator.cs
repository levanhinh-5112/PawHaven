using FluentValidation;

namespace PawHaven.Application.Features.Auth.Commands;

/// <summary>
/// Validator for RegisterUserCommand.
/// Enforces email format and password strength requirements.
/// </summary>
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Length(8, 100)
            .WithMessage("Password must be between 8 and 100 characters.")
            .Matches(@"[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]")
            .WithMessage("Password must contain at least one digit.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.");
    }
}
