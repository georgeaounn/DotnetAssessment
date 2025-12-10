using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class PaginationRequest
    {
        public int PageNumber { get; private set; } = 1;
        public int PageSize { get; private set; } = 10;

        public PaginationRequest() { }

        public PaginationRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
        }
    }
}
