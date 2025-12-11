using FluentValidation;

namespace Application.Features.Orders.Commands.RemoveItemOrder
{
    public class RemoveItemOrderCommandValidator : AbstractValidator<RemoveItemOrderCommand>
    {
        public RemoveItemOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.Request).NotNull();
            RuleFor(x => x.Request.OrderId).NotEmpty().When(x => x.Request != null);
            RuleFor(x => x.Request.ItemId).NotEmpty().When(x => x.Request != null);
        }
    }
}