namespace Application.Features.Auth.Commands.Login.Dtos
{
    public record LoginResponse(string Token, string RefreshToken, Guid UserId, string Email, string Role);
}