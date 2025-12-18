namespace Application.Features.Auth.Commands.Login.Dtos
{
    public record LoginResponse(string Token, string RefreshToken, Guid Id, string Email, string Role);
}