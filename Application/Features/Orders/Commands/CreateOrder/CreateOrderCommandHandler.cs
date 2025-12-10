
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Orders.Dtos;
using Domain.Entities;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<OrderDto>>
    {
        private readonly IItemRepository _items;
        private readonly IOrderRepository _orders;
        private readonly IAuditService _audit;

        public CreateOrderCommandHandler(
            IItemRepository items,
            IOrderRepository orders,
            IAuditService audit)
        {
            _items = items;
            _orders = orders;
            _audit = audit;
        }

        public async Task<Result<OrderDto>> Handle(CreateOrderCommand command, CancellationToken ct = default)
        {
            if (!command.Items.Any())
                return Result<OrderDto>.Failure("Order must contain at least one item");

            var ids = command.Items.Select(i => i.ItemId).ToList();
            var items = await _items.GetByIdsAsync(ids, ct);

            if (items.Count != ids.Count)
                return Result<OrderDto>.Failure("Some items not found");

            var order = new Order() { CustomerId = command.CustomerId };

            foreach (var req in command.Items)
            {
                var item = items.Single(i => i.Id == req.ItemId);
                order.AddItem(item.Id, item.Product.BasePrice);
            }

            await _items.UpdateItemStatus(ids, true, ct);

            await _orders.AddAsync(order, ct);

            await _audit.RecordAsync("CreateOrder", nameof(Order), order.Id.ToString(), command.CustomerId, ct);

            var dtoItems = order.Items
                .Select(oi =>
                {
                    var item = items.Single(i => i.Id == oi.ItemId);
                    return new OrderItemDto(oi.ItemId, item.Name, oi.UnitPrice);
                })
                .ToList();
            return Result<OrderDto>.Success(new OrderDto(order.Id, order.CustomerId, order.CreatedAt, order.TotalPrice, dtoItems));
        }
    }

}