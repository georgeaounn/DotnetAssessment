namespace Application.Features.Items.Commands.CreateItem.Dtos
{
    public record CreateItemRequest(Guid ProductId, string Name);
}
