using FluentValidation;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Request).NotNull();
            RuleFor(x => x.Request.Email).NotEmpty().EmailAddress().When(x => x.Request != null).WithMessage("Valid email is required");
            RuleFor(x => x.Request.Password).NotEmpty().When(x => x.Request != null).WithMessage("Password is required");
        }
    }

}