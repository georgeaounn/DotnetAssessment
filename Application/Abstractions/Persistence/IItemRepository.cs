using Application.Features.Items.Queries.GetAllItems.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Persistence
{
    public interface IItemRepository
    {
        Task AddAsync(Item item, CancellationToken ct);
        Task<(List<Item>, int Total)> GetPaginatedAsync(GetAllItemsRequest Request, CancellationToken ct);
        Task<List<Item>> GetByIdsAsync(List<Guid> ids, CancellationToken ct);
        Task<Item?> GetByIdAsync(Guid id, CancellationToken ct);
        Task UpdateItemStatus(List<Guid> ids, bool Status, CancellationToken ct);
    }
}
