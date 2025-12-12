using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Common;
using Application.Features.Items.Queries.GetAllItems.Dtos;

namespace Application.Features.Items.Queries.GetAllItems
{
    public class GetAllItemsQueryHandler : IQueryHandler<GetAllItemsQuery, Result<PaginationResult<ListItemDto>>>
    {
        private readonly IItemRepository _itemRepository;

        public GetAllItemsQueryHandler(IItemRepository itemRepository) { _itemRepository = itemRepository; }

        public async Task<Result<PaginationResult<ListItemDto>>> Handle(
            GetAllItemsQuery Query,
            CancellationToken ct = default)
        {
            var (Items, TotalCount) = await _itemRepository.GetPaginatedAsync(Query.Request, ct);

            var DtoItems = Items.Select(i => new ListItemDto(i.Id, i.Name, i.ProductId, i.IsSold, i.Product)).ToList();

            var pagedResult = new PaginationResult<ListItemDto>(
                DtoItems,
                TotalCount,
                Query.Request.PageNumber,
                Query.Request.PageSize);

            return Result<PaginationResult<ListItemDto>>.Success(pagedResult);
        }
    }
}