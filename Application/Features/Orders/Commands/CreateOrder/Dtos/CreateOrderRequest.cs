using Application.Features.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.CreateOrder.Dtos
{
    public record CreateOrderRequest(List<OrderItemRequest> Items);
}
