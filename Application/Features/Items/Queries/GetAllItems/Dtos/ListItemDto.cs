using Domain.Entities;
using System;
using System.Linq;

namespace Application.Features.Items.Queries.GetAllItems.Dtos
{
    public record ListItemDto(Guid Id, string Name, Guid ProductId, bool IsSold, Product Product);
}
