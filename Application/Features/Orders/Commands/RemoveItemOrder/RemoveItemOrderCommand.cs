using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Orders.Commands.RemoveItemOrder.Dtos;
using Application.Features.Orders.Dtos;

namespace Application.Features.Orders.Commands.RemoveItemOrder
{
    public record RemoveItemOrderCommand(Guid CustomerId, RemoveItemOrderRequest Request) : ICommand<Result<OrderDto>>;
}