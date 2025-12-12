using Application.Abstractions.CQRS;
using Application.Common;

namespace Application.Features.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(Guid CustomerId, Guid OrderId) : ICommand<Result>;
}