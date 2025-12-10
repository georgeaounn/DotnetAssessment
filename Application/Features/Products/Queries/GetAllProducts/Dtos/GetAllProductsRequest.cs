using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAllProducts.Dtos
{
    public class GetAllProductsRequest : PaginationRequest
    {
        public string? Search { get; set; }
    }
}
