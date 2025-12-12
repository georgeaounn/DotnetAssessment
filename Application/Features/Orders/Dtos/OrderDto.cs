namespace Application.Features.Orders.Dtos
{
    public record OrderDto(Guid Id, Guid CustomerId, DateTime CreatedAt, decimal TotalPrice, List<OrderItemDto> Items);
}