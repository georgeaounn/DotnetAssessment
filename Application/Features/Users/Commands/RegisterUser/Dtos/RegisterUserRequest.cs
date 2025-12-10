namespace Application.Features.Users.Commands.RegisterUser.Dtos
{
    public record RegisterUserRequest(string Name, string Email, string Password, int RoleId);
}