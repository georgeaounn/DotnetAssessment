using Application.Features.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.RemoveItemOrder.Dtos
{
    public record RemoveItemOrderRequest(Guid OrderId, Guid ItemId);
}
