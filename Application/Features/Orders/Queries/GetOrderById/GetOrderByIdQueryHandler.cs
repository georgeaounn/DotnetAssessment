using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Common;
using Application.Features.Orders.Dtos;

namespace Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, Result<OrderDto?>>
    {
        private readonly IOrderRepository _orders;

        public GetOrderByIdQueryHandler(IOrderRepository orders) { _orders = orders; }

        public async Task<Result<OrderDto?>> Handle(GetOrderByIdQuery query, CancellationToken ct = default)
        {
            var order = await _orders.GetById(query.OrderId, ct);
            if(order is null)
                return Result<OrderDto?>.Failure("Invalid order");

            var dtoItems = order.Items.Select(oi => new OrderItemDto(oi.ItemId, oi.Item.Name, oi.UnitPrice)).ToList();

            return Result<OrderDto?>.Success(
                new OrderDto(order.Id, order.CustomerId, order.CreatedAt, order.TotalPrice, dtoItems));
        }
    }
}