using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Items.Queries.GetAllItems.Dtos
{
    public class GetAllItemsRequest : PaginationRequest
    {
        public Guid ProductId { get; set; }
        public string Search { get; set; } = "";
        public bool? IsSold { get; set; }
    }
}
