
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Orders.Dtos;
using Domain.Entities;

namespace Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand, Result>
    {
        private readonly IItemRepository _items;
        private readonly IOrderRepository _orders;
        private readonly IAuditService _audit;

        public DeleteOrderCommandHandler(
            IItemRepository items,
            IOrderRepository orders,
            IAuditService audit)
        {
            _items = items;
            _orders = orders;
            _audit = audit;
        }

        public async Task<Result> Handle(DeleteOrderCommand command, CancellationToken ct = default)
        {
            var order = await _orders.GetById(command.OrderId, ct);
            if (order is null)
                return Result<OrderDto>.Failure("Invalid order");

            if (order.CustomerId != command.CustomerId)
                return Result<OrderDto>.Failure("Cannot delete an order of another user");

            await _items.UpdateItemStatus(order.Items.Select(u => u.ItemId).ToList(), false, ct);

            await _orders.DeleteAsync(order.Id, ct);

            return Result.Success();
        }
    }

}