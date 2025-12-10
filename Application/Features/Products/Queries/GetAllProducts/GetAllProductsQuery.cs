using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Products.Dtos;
using Application.Features.Products.Queries.GetAllProducts.Dtos;

namespace Application.Features.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery(GetAllProductsRequest Request) : IQuery<Result<PaginationResult<ProductDto>>>;
}
