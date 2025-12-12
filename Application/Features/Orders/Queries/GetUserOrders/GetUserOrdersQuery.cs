using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Orders.Dtos;

namespace Application.Features.Orders.Queries.GetUserOrders
{
    public record GetUserOrdersQuery(Guid UserId) : IQuery<Result<List<OrderDto>>>;
}