using FluentValidation;

namespace Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Request).NotNull();
            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(100)
                .When(x => x.Request != null)
                .WithMessage("Name is required and must not exceed 100 characters");
            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .MaximumLength(150)
                .EmailAddress()
                .When(x => x.Request != null)
                .WithMessage("Valid email is required and must not exceed 150 characters");
            RuleFor(x => x.Request.Password)
                .NotEmpty()
                .MinimumLength(6)
                .When(x => x.Request != null)
                .WithMessage("Password must be at least 6 characters");
            RuleFor(x => x.Request.RoleId)
                .Must(id => id == 1 || id == 2)
                .When(x => x.Request != null)
                .WithMessage("RoleId must be 1 (SuperAdmin) or 2 (User).");
        }
    }
}