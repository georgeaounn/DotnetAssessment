using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Users.Commands.RegisterUser.Dtos;

namespace Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IQuery<Result<UserDto?>>;
}