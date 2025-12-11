using Application.Abstractions.CQRS;
using Application.Common;

namespace Application.Features.Items.Commands.DeleteItem
{
    public record DeleteItemCommand(Guid ItemId) : ICommand<Result>;
}

