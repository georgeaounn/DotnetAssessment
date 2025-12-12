using System;
using System.Linq;

namespace Application.Features.Orders.Commands.RemoveItemOrder.Dtos
{
    public record RemoveItemOrderRequest(Guid OrderId, Guid ItemId);
}
