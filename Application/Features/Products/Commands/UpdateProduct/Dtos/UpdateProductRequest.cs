using System;
using System.Linq;

namespace Application.Features.Products.Commands.UpdateProduct.Dtos
{
    public record UpdateProductRequest(string Name, decimal BasePrice, bool IsActive);
}
