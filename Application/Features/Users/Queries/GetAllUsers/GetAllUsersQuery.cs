using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Users.Commands.RegisterUser.Dtos;
using Application.Features.Users.Queries.GetAllUsers.Dtos;

namespace Application.Features.Users.Queries.GetAllUsers
{
    public record GetAllUsersQuery(GetAllUsersRequest Request) : IQuery<Result<PaginationResult<UserDto>>>;
}

