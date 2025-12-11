using Application.Features.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.AddItemOrder.Dtos
{
    public record AddItemOrderRequest(Guid OrderId, Guid ItemId);
}
