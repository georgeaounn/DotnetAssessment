
using Application.Abstractions.Persistence;
using Application.Features.Users.Queries.GetAllUsers.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
            => await _db.Users.AnyAsync(u => u.Email == email, ct);

        public async Task AddAsync(User user, CancellationToken ct)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

        public Task<User?> GetByIdWithRoleAsync(Guid id, CancellationToken ct)
            => _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id, ct);

        public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
            => _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email, ct);

        public Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct)
            => _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, ct);

        public async Task UpdateAsync(User user, CancellationToken ct)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<(List<User>, int Total)> GetPaginatedAsync(GetAllUsersRequest request, CancellationToken ct)
        {
            IQueryable<User> query = _db.Users
                .Include(u => u.Role)
                .AsNoTracking();

            // Search by name or email
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(u =>
                    u.Name.Contains(request.Search) ||
                    u.Email.Contains(request.Search));
            }

            int total = await query.CountAsync(ct);

            var users = await query
                .OrderBy(u => u.Name)
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToListAsync(ct);

            return (users, total);
        }
    }
}