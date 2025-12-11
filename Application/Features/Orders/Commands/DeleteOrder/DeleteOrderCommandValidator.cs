using FluentValidation;

namespace Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required");
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is required");
        }
    }
}

