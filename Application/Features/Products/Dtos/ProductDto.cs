namespace Application.Features.Products.Dtos 
{
    public record ProductDto(Guid Id, string Name, decimal BasePrice, bool IsActive);
}