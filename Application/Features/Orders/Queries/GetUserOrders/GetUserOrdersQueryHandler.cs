
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Orders.Dtos;

namespace Application.Features.Orders.Queries.GetUserOrders
{
    public class GetUserOrdersQueryHandler : IQueryHandler<GetUserOrdersQuery, Result<List<OrderDto>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUser _currentUser;

        public GetUserOrdersQueryHandler(IOrderRepository orderRepository, ICurrentUser currentUser)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUser;
        }

        public async Task<Result<List<OrderDto>>> Handle(GetUserOrdersQuery query, CancellationToken ct = default)
        {
            // Users can only view their own orders unless they are SuperAdmin
            if(!_currentUser.IsSuperAdmin && _currentUser.UserId != query.UserId)
                throw new UnauthorizedAccessException("You can only view your own orders");

            var orders = await _orderRepository.GetByUserIdAsync(query.UserId, ct);

            var orderDtos = orders.Select(
                order => new OrderDto(
                    order.Id,
                    order.CustomerId,
                    order.CreatedAt,
                    order.TotalPrice,
                    order.Items.Select(oi => new OrderItemDto(oi.ItemId, oi.Item.Name, oi.UnitPrice)).ToList()))
                .ToList();

            return Result<List<OrderDto>>.Success(orderDtos);
        }
    }
}