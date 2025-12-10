using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Products.Dtos;

namespace Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(string Name, decimal BasePrice) : ICommand<Result<ProductDto>>;
}