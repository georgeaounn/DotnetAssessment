
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Common;
using Application.Features.Items.Dtos;

namespace Application.Features.Items.Queries.GetItemById 
{
    public class GetItemByIdQueryHandler : IQueryHandler<GetItemByIdQuery, Result<ItemDto?>>
    {
        private readonly IItemRepository _itemRepository;

        public GetItemByIdQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Result<ItemDto?>> Handle(GetItemByIdQuery query, CancellationToken ct = default)
        {
            var item = await _itemRepository.GetByIdAsync(query.ItemId, ct);
            if (item is null) 
                return Result<ItemDto?>.Failure("Invalid item id");

            return Result<ItemDto?>.Success(new ItemDto(item.Id, item.Name, item.ProductId, item.IsSold));
        }
    }


}