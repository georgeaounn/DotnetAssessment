using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Common;
using Application.Features.Items.Dtos;

namespace Application.Features.Items.Queries.GetAllItems
{

    public class GetAllItemsQueryHandler : IQueryHandler<GetAllItemsQuery, Result<PaginationResult<ItemDto>>>
    {
        private readonly IItemRepository _itemRepository;

        public GetAllItemsQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Result<PaginationResult<ItemDto>>> Handle(GetAllItemsQuery Query, CancellationToken ct = default)
        {
            var (Items, TotalCount) = await _itemRepository.GetPaginatedAsync(Query.Request, ct);

            var DtoItems = Items.Select(i => new ItemDto(i.Id, i.Name, i.ProductId, i.IsSold)).ToList();

            var pagedResult = new PaginationResult<ItemDto>(
                DtoItems,
                TotalCount,
                Query.Request.PageNumber,
                Query.Request.PageSize);

            return Result<PaginationResult<ItemDto>>.Success(pagedResult);
        }
    }
}