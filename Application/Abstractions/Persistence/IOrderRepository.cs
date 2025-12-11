using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Persistence
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order, CancellationToken ct);
        Task<Order?> GetById(Guid id, CancellationToken ct);
        Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct);
        Task DeleteAsync(Guid OrderId, CancellationToken ct);
        Task AddItemOrderAsync(Order order, Item item, CancellationToken ct);
        Task RemoveItemOrderAsync(Order order, OrderItem item, CancellationToken ct);

    }
}
