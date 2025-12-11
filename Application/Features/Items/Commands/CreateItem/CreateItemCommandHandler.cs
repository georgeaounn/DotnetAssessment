
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Items.Dtos;
using Domain.Entities;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Features.Items.Commands.CreateItem
{
    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, Result<ItemDto>>
    {
        private readonly IItemRepository _items;
        private readonly IProductRepository _products;
        private readonly ICurrentUser _currentUser;
        private readonly IAuditService _audit;

        public CreateItemCommandHandler(
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

        public async Task<Result<ItemDto>> Handle(CreateItemCommand command, CancellationToken ct = default)
        {
            if (!_currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Only SuperAdmins can create items");

            var product = await _products.GetByIdAsync(command.Request.ProductId, ct);

            if(product is null)
                return Result<ItemDto>.Failure($"Product with id {command.Request.ProductId} not found");

            var item = new Item() { ProductId = product.Id, Name = command.Request.Name};
            await _items.AddAsync(item, ct);

            await _audit.RecordAsync("CreateItem", nameof(Item), item.Id.ToString(), _currentUser.UserId, ct);

            return Result<ItemDto>.Success(new ItemDto(product.Id, product.Name, product.Id));
        }
    }

}