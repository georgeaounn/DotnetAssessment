using FluentValidation;

namespace Application.Features.Items.Queries.GetAllItems
{
    public class GetAllItemsQueryValidator : AbstractValidator<GetAllItemsQuery>
    {
        public GetAllItemsQueryValidator()
        {
            RuleFor(x => x.Request).NotNull();
            RuleFor(x => x.Request.ProductId).NotEmpty().When(x => x.Request != null).WithMessage("ProductId is required");
        }
    }
}

