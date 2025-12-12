using Application.Abstractions.Persistence;
using Application.Features.Items.Queries.GetAllItems.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{

    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _db;

        public ItemRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Item item, CancellationToken ct)
        {
            _db.Items.Add(item);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<List<Item>> GetByIdsAsync(List<Guid> ids, CancellationToken ct)
        {
            var list = ids.ToList();
            return await _db.Items.Where(i => list.Contains(i.Id)).Include(u => u.Product).ToListAsync(ct);
        }

        public async Task<(List<Item>, int Total)> GetPaginatedAsync(GetAllItemsRequest Request, CancellationToken ct)
        {
            IQueryable<Item> Query = _db.Items.AsNoTracking();

            if (Request.ProductId.HasValue)
                Query = Query.Where(i => i.ProductId == Request.ProductId);

            // Search by name
            if (!string.IsNullOrWhiteSpace(Request.Search))
                Query = Query.Where(i => i.Name.Contains(Request.Search));

            // Filter by IsSold
            if (Request.IsSold.HasValue)
                Query = Query.Where(i => i.IsSold == Request.IsSold.Value);

            int Total = await Query.CountAsync();

            return (await Query.OrderBy(i => i.Name).Include(i => i.Product).Skip(Request.PageSize * (Request.PageNumber - 1)).Take(Request.PageSize).ToListAsync(ct), Total);
        }


        public Task<Item?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Items.Include(i => i.Product).FirstOrDefaultAsync(i => i.Id == id, ct);


        public async Task UpdateItemStatus(List<Guid> ids, bool status, CancellationToken ct)
        {
            var items = await _db.Items
                .Where(i => ids.Contains(i.Id))
                .ToListAsync(ct);

            foreach (var item in items)
            {
                item.IsSold = status;
            }

            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Item item, CancellationToken ct)
        {
            _db.Items.Update(item);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Item item, CancellationToken ct)
        {
            _db.Items.Remove(item);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<bool> HasSoldItemsByProductIdAsync(Guid productId, CancellationToken ct)
        {
            return await _db.Items
                .AnyAsync(i => i.ProductId == productId && i.IsSold == true, ct);
        }
    }


}
