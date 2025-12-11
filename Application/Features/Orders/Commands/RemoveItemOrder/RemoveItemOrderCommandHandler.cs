
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Orders.Dtos;
using Domain.Entities;

namespace Application.Features.Orders.Commands.RemoveItemOrder
{
    public class RemoveItemOrderCommandHandler : ICommandHandler<RemoveItemOrderCommand, Result<OrderDto>>
    {
        private readonly IItemRepository _items;
        private readonly IOrderRepository _orders;
        private readonly IAuditService _audit;

        public RemoveItemOrderCommandHandler(
            IItemRepository items,
            IOrderRepository orders,
            IAuditService audit)
        {
            _items = items;
            _orders = orders;
            _audit = audit;
        }

        public async Task<Result<OrderDto>> Handle(RemoveItemOrderCommand command, CancellationToken ct = default)
        {
            var order = await _orders.GetById(command.Request.OrderId, ct); 
            if (order is null) 
                return Result<OrderDto>.Failure("Invalid order id"); 

            if (order.CustomerId != command.CustomerId) 
                return Result<OrderDto>.Failure("Cannot edit an order of another user"); 
            
            var item = await _items.GetByIdAsync(command.Request.ItemId, ct); 

            if (item is null) 
                return Result<OrderDto>.Failure("Invalid item id");

            var orderItem = order.Items.FirstOrDefault(u => u.ItemId == item.Id);
            if (orderItem is null) 
                return Result<OrderDto>.Failure("This item is not in this order"); 
            
            await _items.UpdateItemStatus(new List<Guid>() { item.Id }, false, ct); 
           
            await _orders.RemoveItemOrderAsync(order,orderItem, ct);

            await _audit.RecordAsync("RemoveItemOrder", nameof(Order), order.Id.ToString(), command.CustomerId, ct); 
            
            var dtoItems = order.Items.Select(oi => { return new OrderItemDto(oi.ItemId, item.Name, oi.UnitPrice); }).ToList(); 
            
            return Result<OrderDto>.Success(new OrderDto(order.Id, order.CustomerId, order.CreatedAt, order.TotalPrice, dtoItems));
        }
    }

}