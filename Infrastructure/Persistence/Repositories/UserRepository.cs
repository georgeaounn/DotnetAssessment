
using Application.Abstractions.Persistence;
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

        public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
            => _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email, ct);

        public async Task UpdateAsync(User user, CancellationToken ct)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync(ct);
        }
    }


}