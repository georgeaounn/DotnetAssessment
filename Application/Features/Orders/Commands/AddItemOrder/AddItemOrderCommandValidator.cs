using Application.Features.Orders.Commands.AddItemOrder.Dtos;
using FluentValidation;

namespace Application.Features.Orders.Commands.AddItemOrder
{
    public class AddItemOrderCommandValidator : AbstractValidator<AddItemOrderRequest>
    {
        public AddItemOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}