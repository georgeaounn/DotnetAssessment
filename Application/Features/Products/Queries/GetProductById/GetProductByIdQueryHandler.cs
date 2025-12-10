using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Common;
using Application.Features.Products.Dtos;

namespace Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Result<ProductDto?>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductDto?>> Handle(GetProductByIdQuery query, CancellationToken ct = default)
        {
            var product = await _productRepository.GetByIdAsync(query.ProductId, ct);
            if (product is null) 
                return Result<ProductDto?>.Failure("Invalid product id");

            return Result<ProductDto?>.Success( new ProductDto(product.Id, product.Name, product.BasePrice));
        }
    }

}