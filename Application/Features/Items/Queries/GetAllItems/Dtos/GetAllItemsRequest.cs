using Application.Common;
using System;
using System.Linq;

namespace Application.Features.Items.Queries.GetAllItems.Dtos
{
    public class GetAllItemsRequest : PaginationRequest
    {
        public Guid? ProductId { get; set; }
        public string Search { get; set; } = string.Empty;
        public bool? IsSold { get; set; }
    }
}
