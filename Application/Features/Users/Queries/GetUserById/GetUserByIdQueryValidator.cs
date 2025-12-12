using FluentValidation;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator() 
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required"); 
        }
    }
}

