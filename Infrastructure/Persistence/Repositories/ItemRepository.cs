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
            return await _db.Items.Where(i => list.Contains(i.Id)).ToListAsync(ct);
        }

        public async Task<(List<Item>, int Total)> GetPaginatedAsync(GetAllItemsRequest Request, CancellationToken ct)
        {
            IQueryable<Item> Query = _db.Items.AsNoTracking().Where(i => i.ProductId == Request.ProductId);

            // Search by name
            if(!string.IsNullOrWhiteSpace(Request.Search))
                Query = Query.Where(i => i.Name.Contains(Request.Search));

            int Total = await Query.CountAsync();

            return (await Query.OrderBy(i => i.Name).ToListAsync(ct), Total);
        }


        public Task<Item?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Items.Include(i => i.Product).FirstOrDefaultAsync(i => i.Id == id, ct);


        public async Task UpdateItemStatus(List<Guid> ids, bool status, CancellationToken ct)
        {
            await _db.Items
                .Where(i => ids.Contains(i.Id))
                .ExecuteUpdateAsync(
                    setter => setter
                        .SetProperty(i => i.IsSold, it => status),
                    ct);
        }
    }


}
