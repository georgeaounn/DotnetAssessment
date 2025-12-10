using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Products.Dtos;

namespace Application.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(Guid ProductId) : IQuery<Result<ProductDto?>>;
}