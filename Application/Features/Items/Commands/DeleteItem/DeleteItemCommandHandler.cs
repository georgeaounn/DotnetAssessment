using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;

namespace Application.Features.Items.Commands.DeleteItem
{
    public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand, Result>
    {
        private readonly IItemRepository _items;
        private readonly ICurrentUser _currentUser;
        private readonly IAuditService _audit;

        public DeleteItemCommandHandler(
            IItemRepository items,
            ICurrentUser currentUser,
            IAuditService audit)
        {
            _items = items;
            _currentUser = currentUser;
            _audit = audit;
        }

        public async Task<Result> Handle(DeleteItemCommand command, CancellationToken ct = default)
        {
            if (!_currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Only SuperAdmins can delete items");

            var item = await _items.GetByIdAsync(command.ItemId, ct);
            if (item is null)
                return Result.Failure("Item not found");

            if (item.IsSold)
                return Result.Failure("Cannot delete item. Item has IsSold=true.");

            await _items.DeleteAsync(item, ct);

            await _audit.RecordAsync("DeleteItem", nameof(Domain.Entities.Item), item.Id.ToString(), _currentUser.UserId, ct);

            return Result.Success();
        }
    }
}

