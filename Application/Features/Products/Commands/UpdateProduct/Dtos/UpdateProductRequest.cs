using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.UpdateProduct.Dtos
{
    public record UpdateProductRequest(string Name, decimal BasePrice, bool IsActive);
}
