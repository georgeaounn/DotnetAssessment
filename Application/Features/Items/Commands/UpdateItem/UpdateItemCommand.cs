using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Items.Commands.UpdateItem.Dtos;
using Application.Features.Items.Dtos;

namespace Application.Features.Items.Commands.UpdateItem
{
    public record UpdateItemCommand(Guid ItemId, UpdateItemRequest Request) : ICommand<Result<ItemDto>>;
}

