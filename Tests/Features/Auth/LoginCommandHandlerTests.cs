using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Login.Dtos;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Features.Auth
{
    public class LoginCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenUserNotFound_ReturnsFailure()
        {
            var userRepo = new Mock<IUserRepository>();
            var passwordHasher = new Mock<IPasswordHasher>();
            var jwtService = new Mock<IJwtTokenService>();
            var handler = new LoginCommandHandler(userRepo.Object, passwordHasher.Object, jwtService.Object);

            var command = new LoginCommand(new LoginRequest("test@example.com", "password"));

            userRepo.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var result = await handler.Handle(command);

            Assert.True(result.IsFailure);
            Assert.Equal("Invalid email or password", result.Error);
        }

        [Fact]
        public async Task Handle_WhenPasswordInvalid_ReturnsFailure()
        {
            var userRepo = new Mock<IUserRepository>();
            var passwordHasher = new Mock<IPasswordHasher>();
            var jwtService = new Mock<IJwtTokenService>();
            var handler = new LoginCommandHandler(userRepo.Object, passwordHasher.Object, jwtService.Object);

            var command = new LoginCommand(new LoginRequest("test@example.com", "wrongpassword"));

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Password = "hashedpassword",
                Role = new Role { Id = 1, Name = "User" }
            };

            userRepo.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            passwordHasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var result = await handler.Handle(command);

            Assert.True(result.IsFailure);
            Assert.Equal("Invalid email or password", result.Error);
        }

        [Fact]
        public async Task Handle_WhenValidCredentials_ReturnsSuccess()
        {
            var userRepo = new Mock<IUserRepository>();
            var passwordHasher = new Mock<IPasswordHasher>();
            var jwtService = new Mock<IJwtTokenService>();
            var handler = new LoginCommandHandler(userRepo.Object, passwordHasher.Object, jwtService.Object);

            var userId = Guid.NewGuid();
            var command = new LoginCommand(new LoginRequest("test@example.com", "password"));

            var role = new Role { Id = 1, Name = "User" };
            var user = new User
            {
                Id = userId,
                Email = "test@example.com",
                Password = "hashedpassword",
                RoleId = 1,
                Role = role
            };

            userRepo.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            passwordHasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            jwtService.Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns("jwt-token");

            jwtService.Setup(x => x.GenerateRefreshToken()).Returns("refresh-token");

            userRepo.Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await handler.Handle(command);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("jwt-token", result.Data.Token);
            Assert.Equal("refresh-token", result.Data.RefreshToken);
            Assert.Equal(userId, result.Data.Id);
            Assert.Equal("test@example.com", result.Data.Email);

            userRepo.Verify(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}