using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Users.Commands.RegisterUser.Dtos;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Result<UserDto?>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public GetUserByIdQueryHandler(IUserRepository userRepository, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<Result<UserDto?>> Handle(GetUserByIdQuery query, CancellationToken ct = default)
        {
            if(!_currentUser.IsSuperAdmin && _currentUser.UserId != query.UserId)
                throw new UnauthorizedAccessException("Only SuperAdmins can view user details");

            var user = await _userRepository.GetByIdWithRoleAsync(query.UserId, ct);
            if(user == null)
                return Result<UserDto?>.Failure("User not found");

            return Result<UserDto?>.Success(new UserDto(user.Id, user.Name, user.Email, user.Role.Name));
        }
    }
}

