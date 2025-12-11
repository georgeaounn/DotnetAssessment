using Application.Features.Orders.Commands.CreateOrder.Dtos;
using FluentValidation;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Items).NotEmpty();
            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.ItemId).NotEmpty();
            });
        }
    }
}