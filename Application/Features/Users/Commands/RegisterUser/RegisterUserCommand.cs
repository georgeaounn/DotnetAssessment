
using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Users.Commands.RegisterUser.Dtos;

namespace Application.Features.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterUserRequest Request) : ICommand<Result<UserDto>>;
}