
using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Items.Commands.CreateItem.Dtos;
using Application.Features.Items.Dtos;

namespace Application.Features.Items.Commands.CreateItem
{
    public record CreateItemCommand(CreateItemRequest Request) : ICommand<Result<ItemDto>>;
}