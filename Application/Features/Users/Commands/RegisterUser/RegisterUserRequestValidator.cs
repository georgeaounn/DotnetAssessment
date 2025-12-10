using Application.Features.Users.Commands.RegisterUser.Dtos;
using FluentValidation;

namespace Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.RoleId).Must(id => id == 1 || id == 2)
                .WithMessage("RoleId must be 1 (SuperAdmin) or 2 (User).");
        }
    }

}