using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Orders.Commands.AddItemOrder.Dtos;
using Application.Features.Orders.Dtos;

namespace Application.Features.Orders.Commands.AddItemOrder
{
    public record AddItemOrderCommand(Guid CustomerId, AddItemOrderRequest Request) : ICommand<Result<OrderDto>>;
}