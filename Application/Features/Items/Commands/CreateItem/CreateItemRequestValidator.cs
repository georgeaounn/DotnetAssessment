using Application.Features.Items.Commands.CreateItem.Dtos;
using Application.Features.Users.Commands.RegisterUser.Dtos;
using FluentValidation;

namespace Application.Features.Items.Commands.CreateItem
{
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}