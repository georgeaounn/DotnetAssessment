using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Auth.Commands.Login.Dtos;

namespace Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result<LoginResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand command, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(command.RefreshToken, ct);
            if (user == null)
                return Result<LoginResponse>.Failure("Invalid refresh token");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Result<LoginResponse>.Failure("Refresh token has expired");

            var newToken = _jwtTokenService.GenerateToken(user.Id, user.Email, user.RoleId);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user, ct);

            return Result<LoginResponse>.Success(
                new LoginResponse(
                    newToken,
                    newRefreshToken,
                    user.Id,
                    user.Email,
                    user.Role.Name));
        }
    }
}

