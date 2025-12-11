using Application.Abstractions.CQRS;
using Application.Common;
using Application.Features.Auth.Commands.Login.Dtos;

namespace Application.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string Token, string RefreshToken) : ICommand<Result<LoginResponse>>;
}

