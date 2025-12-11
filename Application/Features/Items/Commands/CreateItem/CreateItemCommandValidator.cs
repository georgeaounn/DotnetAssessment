using FluentValidation;

namespace Application.Features.Items.Commands.CreateItem
{
    public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        public CreateItemCommandValidator()
        {
            RuleFor(x => x.Request).NotNull();
            RuleFor(x => x.Request.Name).NotEmpty().When(x => x.Request != null).WithMessage("Name is required");
            RuleFor(x => x.Request.ProductId).NotEmpty().When(x => x.Request != null).WithMessage("ProductId is required");
        }
    }
}

