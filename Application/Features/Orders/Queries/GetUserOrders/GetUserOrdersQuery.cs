using Application.Abstractions.CQRS;
using Application.Features.Orders.Dtos;

namespace Application.Features.Orders.Queries.GetUserOrders
{
    public record GetUserOrdersQuery(Guid UserId) : IQuery<List<OrderDto>>;
}