
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Auth.Commands.Login.Dtos;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken ct = default)
        {
            var request = command.Request;

            var user = await _userRepository.GetByEmailAsync(request.Email, ct);
            if (user is null)
                return Result<LoginResponse>.Failure("Invalid email or password");

            if (!_passwordHasher.Verify(request.Password, user.Password))
                return Result<LoginResponse>.Failure("Invalid email or password");

            var token = _jwtTokenService.GenerateToken(user.Id, user.Email, user.RoleId);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user, ct);

            return Result<LoginResponse>.Success(new LoginResponse(token, refreshToken, user.Id, user.Email, user.Role.Name));
        }
    }
}