using Application.Common;

namespace Application.Features.Users.Queries.GetAllUsers.Dtos
{
    public class GetAllUsersRequest : PaginationRequest
    {
        public string? Search { get; set; }
    }
}

