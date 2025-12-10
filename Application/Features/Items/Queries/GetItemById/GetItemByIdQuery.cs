using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Items.Dtos;

namespace Application.Features.Items.Queries.GetItemById
{
    public record GetItemByIdQuery(Guid ItemId) : IQuery<Result<ItemDto?>>;
}