using System;
using System.Linq;

namespace Application.Features.Auth.Commands.RefreshToken.Dtos
{
    public record RefreshTokenRequest(string Token, string RefreshToken);
}
