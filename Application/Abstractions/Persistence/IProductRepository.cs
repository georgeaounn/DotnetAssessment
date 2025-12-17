using Application.Common;
using Application.Features.Products.Queries.GetAllProducts.Dtos;
using Domain.Entities;

namespace Application.Abstractions.Persistence
{
    public interface IProductRepository
    {
        Task AddAsync(Product product, CancellationToken ct);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<bool> NameExistsAsync(string Name, CancellationToken ct);
        Task<(List<Product>, int Total)> GetPaginatedAsync(GetAllProductsRequest Request, CancellationToken ct);
        Task UpdateAsync(Product product, CancellationToken ct);
        Task DeleteAsync(Product product, CancellationToken ct);
    }
}