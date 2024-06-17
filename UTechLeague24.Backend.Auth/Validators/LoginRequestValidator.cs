using FluentValidation;
using UTechLeague24.Backend.Auth.Models;

namespace UTechLeague24.Backend.Auth.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(5, 50).WithMessage("Username must be between 5 and 50 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 100).WithMessage("Password must be between 8 and 100 characters.");
    }
}