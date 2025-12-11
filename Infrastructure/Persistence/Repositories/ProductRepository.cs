
using Application.Abstractions.Persistence;
using Application.Common;
using Application.Features.Items.Queries.GetAllItems.Dtos;
using Application.Features.Products.Queries.GetAllProducts.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories 
{

    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Product product, CancellationToken ct)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync(ct);
        }

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
        public Task<bool> NameExistsAsync(string Name, CancellationToken ct)
            => _db.Products.AnyAsync(p => p.Name == Name, ct);

        public async Task<(List<Product>, int Total)> GetPaginatedAsync(GetAllProductsRequest Request, CancellationToken ct)
        {
            IQueryable<Product> Query = _db.Products.AsNoTracking();

            // Search by name
            if (!string.IsNullOrWhiteSpace(Request.Search))
                Query = Query.Where(i => i.Name.Contains(Request.Search));

            int Total = await Query.CountAsync();

            return (await Query.OrderBy(i => i.Name).Skip(Request.PageSize * (Request.PageNumber - 1)).Take(Request.PageSize).ToListAsync(ct), Total);
        }
    }

}