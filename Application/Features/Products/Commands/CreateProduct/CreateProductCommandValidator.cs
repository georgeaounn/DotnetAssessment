using FluentValidation;

namespace Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150).WithMessage("Product name is required and must not exceed 150 characters");
            RuleFor(x => x.BasePrice).GreaterThan(0).WithMessage("Base price must be greater than 0");
        }
    }
}

