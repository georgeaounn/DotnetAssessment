namespace Application.Features.Items.Commands.UpdateItem.Dtos
{
    public record UpdateItemRequest(Guid ItemId, string Name, Guid ProductId);
}

