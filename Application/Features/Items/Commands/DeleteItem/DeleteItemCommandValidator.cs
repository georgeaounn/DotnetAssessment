using FluentValidation;

namespace Application.Features.Items.Commands.DeleteItem
{
    public class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
    {
        public DeleteItemCommandValidator()
        {
            RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId is required");
        }
    }
}

