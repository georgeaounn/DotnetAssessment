
using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Orders.Dtos;

namespace Application.Features.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery(Guid OrderId) : IQuery<Result<OrderDto?>>;
}