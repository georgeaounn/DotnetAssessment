using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Products.Dtos;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(Guid ProductId, string Name, decimal BasePrice, bool IsActive) : ICommand<Result<ProductDto>>;
}

