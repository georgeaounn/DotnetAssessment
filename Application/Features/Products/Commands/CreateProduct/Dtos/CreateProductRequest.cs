using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.CreateProduct.Dtos
{
    public record CreateProductRequest(string Name, decimal BasePrice);
}
