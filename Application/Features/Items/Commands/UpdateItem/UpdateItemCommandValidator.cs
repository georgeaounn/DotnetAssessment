using FluentValidation;

namespace Application.Features.Items.Commands.UpdateItem
{
    public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
    {
        public UpdateItemCommandValidator()
        {
            RuleFor(x => x.Request).NotNull();
            RuleFor(x => x.ItemId)
                .NotEmpty()
                .WithMessage("ItemId is required");
            RuleFor(x => x.Request.Name).NotEmpty().When(x => x.Request != null).WithMessage("Name is required");
            RuleFor(x => x.Request.ProductId)
                .NotEmpty()
                .When(x => x.Request != null)
                .WithMessage("ProductId is required");
        }
    }
}


