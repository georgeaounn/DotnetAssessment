using Application.Features.Orders.Commands.RemoveItemOrder.Dtos;
using FluentValidation;

namespace Application.Features.Orders.Commands.RemoveItemOrder
{
    public class RemoveItemOrderCommandValidator : AbstractValidator<RemoveItemOrderRequest>
    {
        public RemoveItemOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}