using Application.Common;
using System;
using System.Linq;

namespace Application.Features.Products.Queries.GetAllProducts.Dtos
{
    public class GetAllProductsRequest : PaginationRequest
    {
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
    }
}
