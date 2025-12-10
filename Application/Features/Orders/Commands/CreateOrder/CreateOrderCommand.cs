using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Orders.Dtos;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(Guid CustomerId, List<OrderItemRequest> Items) : ICommand<Result<OrderDto>>;
}