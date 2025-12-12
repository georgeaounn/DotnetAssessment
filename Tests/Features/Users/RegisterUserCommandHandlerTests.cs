using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Features.Users.Commands.RegisterUser;
using Application.Features.Users.Commands.RegisterUser.Dtos;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Features.Users;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenRoleNotFound_ReturnsFailure()
    {
        var userRepo = new Mock<IUserRepository>();
        var roleRepo = new Mock<IRoleRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var auditService = new Mock<IAuditService>();
        var logger = new Mock<ILogger<RegisterUserCommandHandler>>();
        var handler = new RegisterUserCommandHandler(userRepo.Object, roleRepo.Object, passwordHasher.Object, auditService.Object, logger.Object);
        
        var command = new RegisterUserCommand(new RegisterUserRequest("Test User", "test@example.com", "password", 1));
        
        roleRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role?)null);

        var result = await handler.Handle(command);

        Assert.True(result.IsFailure);
        Assert.Equal("Invalid role", result.Error);
    }

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ReturnsFailure()
    {
        var userRepo = new Mock<IUserRepository>();
        var roleRepo = new Mock<IRoleRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var auditService = new Mock<IAuditService>();
        var logger = new Mock<ILogger<RegisterUserCommandHandler>>();
        var handler = new RegisterUserCommandHandler(userRepo.Object, roleRepo.Object, passwordHasher.Object, auditService.Object, logger.Object);
        
        var command = new RegisterUserCommand(new RegisterUserRequest("Test User", "test@example.com", "password", 1));
        
        var role = new Role { Id = 1, Name = "User" };
        roleRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);
        
        userRepo.Setup(x => x.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await handler.Handle(command);

        Assert.True(result.IsFailure);
        Assert.Equal("Email already registered", result.Error);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ReturnsSuccess()
    {
        var userRepo = new Mock<IUserRepository>();
        var roleRepo = new Mock<IRoleRepository>();
        var passwordHasher = new Mock<IPasswordHasher>();
        var auditService = new Mock<IAuditService>();
        var logger = new Mock<ILogger<RegisterUserCommandHandler>>();
        var handler = new RegisterUserCommandHandler(userRepo.Object, roleRepo.Object, passwordHasher.Object, auditService.Object, logger.Object);
        
        var command = new RegisterUserCommand(new RegisterUserRequest("Test User", "test@example.com", "password", 1));
        
        var role = new Role { Id = 1, Name = "User" };
        roleRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);
        
        userRepo.Setup(x => x.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        passwordHasher.Setup(x => x.Hash(It.IsAny<string>()))
            .Returns("hashed-password");
        
        userRepo.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        auditService.Setup(x => x.RecordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await handler.Handle(command);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("Test User", result.Data.Name);
        Assert.Equal("test@example.com", result.Data.Email);
        Assert.Equal("User", result.Data.Role);
        
        userRepo.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        auditService.Verify(x => x.RecordAsync("RegisterUser", "User", It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

