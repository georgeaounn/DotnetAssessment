using Application.Features.Orders.Dtos;
using System;
using System.Linq;

namespace Application.Features.Orders.Commands.CreateOrder.Dtos
{
    public record CreateOrderRequest(List<OrderItemRequest> Items);
}
