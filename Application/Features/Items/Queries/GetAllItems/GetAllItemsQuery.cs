using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Items.Dtos;
using Application.Features.Items.Queries.GetAllItems.Dtos;

namespace Application.Features.Items.Queries.GetAllItems
{
    public record GetAllItemsQuery(GetAllItemsRequest Request) : IQuery<Result<PaginationResult<ItemDto>>>;
}