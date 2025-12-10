namespace Application.Features.Orders.Dtos
{
    public record OrderItemDto(Guid ItemId, string ItemName, decimal UnitPrice);

}