using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Users.Commands.RegisterUser.Dtos;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<UserDto>>
{
    private readonly IUserRepository _users;
    private readonly IRoleRepository _roles;
    private readonly IPasswordHasher _hasher;
    private readonly IAuditService _audit;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        IUserRepository users,
        IRoleRepository roles,
        IPasswordHasher hasher,
        IAuditService audit,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _users = users;
        _roles = roles;
        _hasher = hasher;
        _audit = audit;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(RegisterUserCommand command, CancellationToken ct = default)
    {
        var req = command.Request;

        var role = await _roles.GetByIdAsync(req.RoleId, ct);
        if(role is null)
            return Result<UserDto>.Failure("Invalid role");

        if(await _users.EmailExistsAsync(req.Email, ct))
            return Result<UserDto>.Failure("Email already registered");

        var hash = _hasher.Hash(req.Password);
        var user = new User() { Name = req.Name, Email = req.Email, Password = hash, RoleId = req.RoleId };

        await _users.AddAsync(user, ct);

        await _audit.RecordAsync("RegisterUser", nameof(User), user.Id.ToString(), user.Id, ct);

        _logger.LogInformation("User {UserId} registered", user.Id);

        return Result<UserDto>.Success(new UserDto(user.Id, user.Name, user.Email, role.Name));
    }
}

