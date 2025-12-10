

using Application.Abstractions.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{

    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Order order, CancellationToken ct)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct);
        }

        public Task<Order?> GetById(Guid id, CancellationToken ct)
            => _db.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(o => o.Id == id, ct);

        public Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct)
            => _db.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Item)
                .Where(o => o.CustomerId == userId)
                .ToListAsync(ct);
    }
}