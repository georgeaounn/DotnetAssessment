using FluentValidation;

namespace Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
    {
        public GetAllUsersQueryValidator()
        {
            RuleFor(x => x.Request).NotNull();
            RuleFor(x => x.Request.PageNumber)
                .GreaterThan(0)
                .When(x => x.Request != null)
                .WithMessage("PageNumber must be greater than 0");
            RuleFor(x => x.Request.PageSize)
                .GreaterThan(0)
                .When(x => x.Request != null)
                .WithMessage("PageSize must be greater than 0");
        }
    }
}

