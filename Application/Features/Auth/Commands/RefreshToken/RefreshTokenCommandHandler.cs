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
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(command.Token);
            if(principal == null)
                return Result<LoginResponse>.Failure("Invalid token");

            var userIdClaim = principal.FindFirst("UserId")?.Value;
            if(userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Result<LoginResponse>.Failure("Invalid token");

            var user = await _userRepository.GetByIdAsync(userId, ct);
            if(user == null)
                return Result<LoginResponse>.Failure("User not found");

            // Load user with Role
            var userWithRole = await _userRepository.GetByEmailAsync(user.Email, ct);
            if(userWithRole == null)
                return Result<LoginResponse>.Failure("User not found");

            if(userWithRole.RefreshToken != command.RefreshToken)
                return Result<LoginResponse>.Failure("Invalid refresh token");

            if(userWithRole.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Result<LoginResponse>.Failure("Refresh token has expired");

            var newToken = _jwtTokenService.GenerateToken(userWithRole.Id, userWithRole.Email, userWithRole.RoleId);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            userWithRole.RefreshToken = newRefreshToken;
            userWithRole.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(userWithRole, ct);

            return Result<LoginResponse>.Success(
                new LoginResponse(
                    newToken,
                    newRefreshToken,
                    userWithRole.Id,
                    userWithRole.Email,
                    userWithRole.Role.Name));
        }
    }
}

