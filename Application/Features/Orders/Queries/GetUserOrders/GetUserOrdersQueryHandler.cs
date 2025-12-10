
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Features.Orders.Dtos;

namespace Application.Features.Orders.Queries.GetUserOrders
{
    public class GetUserOrdersQueryHandler : IQueryHandler<GetUserOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetUserOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderDto>> Handle(GetUserOrdersQuery query, CancellationToken ct = default)
        {
            var orders = await _orderRepository.GetByUserIdAsync(query.UserId, ct);

            return orders.Select(order => new OrderDto(
                order.Id,
                order.CustomerId,
                order.CreatedAt,
                order.TotalPrice,
                order.Items.Select(oi => new OrderItemDto(
                    oi.ItemId,
                    oi.Item.Name,
                    oi.UnitPrice)).ToList())).ToList();
        }
    }

}