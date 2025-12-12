using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Users.Commands.RegisterUser.Dtos;

namespace Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, Result<PaginationResult<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public GetAllUsersQueryHandler(IUserRepository userRepository, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<Result<PaginationResult<UserDto>>> Handle(
            GetAllUsersQuery query,
            CancellationToken ct = default)
        {
            if(!_currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Only SuperAdmins can view all users");

            var (users, totalCount) = await _userRepository.GetPaginatedAsync(query.Request, ct);

            var dtoUsers = users.Select(u => new UserDto(u.Id, u.Name, u.Email, u.Role.Name)).ToList();

            var pagedResult = new PaginationResult<UserDto>(
                dtoUsers,
                totalCount,
                query.Request.PageNumber,
                query.Request.PageSize);

            return Result<PaginationResult<UserDto>>.Success(pagedResult);
        }
    }
}

