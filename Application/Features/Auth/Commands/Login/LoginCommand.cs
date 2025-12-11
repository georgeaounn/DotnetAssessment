using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Auth.Commands.Login.Dtos;

namespace Application.Features.Auth.Commands.Login
{
    public record LoginCommand(LoginRequest Request) : ICommand<Result<LoginResponse>>;

}