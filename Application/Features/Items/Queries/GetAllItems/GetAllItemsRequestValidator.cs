using Application.Features.Items.Queries.GetAllItems.Dtos;
using FluentValidation;

namespace Application.Features.Items.Queries.GetAllItems
{
    public class GetAllItemsRequestValidator : AbstractValidator<GetAllItemsRequest>
    {
        public GetAllItemsRequestValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}