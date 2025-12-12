using System;
using System.Linq;

namespace Application.Features.Orders.Commands.AddItemOrder.Dtos
{
    public record AddItemOrderRequest(Guid OrderId, Guid ItemId);
}
