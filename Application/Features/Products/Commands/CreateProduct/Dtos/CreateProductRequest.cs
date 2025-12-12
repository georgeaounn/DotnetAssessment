using System;
using System.Linq;

namespace Application.Features.Products.Commands.CreateProduct.Dtos
{
    public record CreateProductRequest(string Name, decimal BasePrice);
}
