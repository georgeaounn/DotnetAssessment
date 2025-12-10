
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Common;
using Application.Features.Items.Dtos;
using Application.Features.Items.Queries.GetAllItems;
using Application.Features.Products.Dtos;

namespace Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, Result<PaginationResult<ProductDto>>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<PaginationResult<ProductDto>>> Handle(GetAllProductsQuery Query, CancellationToken ct = default)
    {
        var (products, TotalCount) = await _productRepository.GetPaginatedAsync(Query.Request, ct);

        var dtoProducts = products.Select(p => new ProductDto(p.Id, p.Name, p.BasePrice)).ToList();

        var pagedResult = new PaginationResult<ProductDto>(
            dtoProducts,
            TotalCount,
            Query.Request.PageNumber,
            Query.Request.PageSize);

        return Result<PaginationResult<ProductDto>>.Success(pagedResult);
    }
}