namespace Application.Features.Auth.Commands.Login.Dtos
{
    public record LoginResponse(string Token, Guid UserId, string Email, string Role);
}