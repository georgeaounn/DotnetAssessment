using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Items.Dtos;

namespace Application.Features.Items.Commands.UpdateItem
{
    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, Result<ItemDto>>
    {
        private readonly IItemRepository _items;
        private readonly IProductRepository _products;
        private readonly ICurrentUser _currentUser;
        private readonly IAuditService _audit;

        public UpdateItemCommandHandler(
            IItemRepository items,
            IProductRepository products,
            ICurrentUser currentUser,
            IAuditService audit)
        {
            _items = items;
            _products = products;
            _currentUser = currentUser;
            _audit = audit;
        }

        public async Task<Result<ItemDto>> Handle(UpdateItemCommand command, CancellationToken ct = default)
        {
            if(!_currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Only SuperAdmins can update items");

            var item = await _items.GetByIdAsync(command.ItemId, ct);
            if(item is null)
                return Result<ItemDto>.Failure("Item not found");

            var product = await _products.GetByIdAsync(command.Request.ProductId, ct);
            if(product is null)
                return Result<ItemDto>.Failure("Product not found");

            item.Name = command.Request.Name;
            item.ProductId = command.Request.ProductId;

            await _items.UpdateAsync(item, ct);


            return Result<ItemDto>.Success(new ItemDto(item.Id, item.Name, item.ProductId, item.IsSold));
        }
    }
}

