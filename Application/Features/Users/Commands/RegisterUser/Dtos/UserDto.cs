namespace Application.Features.Users.Commands.RegisterUser.Dtos;

public record UserDto(Guid Id, string Name, string Email, string Role);

