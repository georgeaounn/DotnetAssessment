using Application.Features.Auth.Commands.Login.Dtos;
using FluentValidation;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginUserRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

}