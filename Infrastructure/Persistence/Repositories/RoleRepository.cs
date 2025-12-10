using Application.Abstractions.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{

    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _db;

        public RoleRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<Role?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Roles.FirstOrDefaultAsync(r => r.Id == id, ct);
    }


}