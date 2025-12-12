using FluentValidation;

namespace Application.Features.Items.Queries.GetItemById
{
    public class GetItemByIdQueryValidator : AbstractValidator<GetItemByIdQuery>
    {
        public GetItemByIdQueryValidator() { RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId is required"); }
    }
}

