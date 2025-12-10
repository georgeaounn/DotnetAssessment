using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{

    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;

        public DatabaseSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedRolesAsync(CancellationToken ct = default)
        {
            if (await _context.Roles.AnyAsync(ct))
                return; // Roles already seeded

            var roles = new[]
            {
                new Role(){ Id =  1, Name = "SuperAdmin" },
                new Role() { Id = 2, Name = "User" }
            };

            _context.Roles.AddRange(roles);
            await _context.SaveChangesAsync(ct);
        }

        public async Task SeedAsync(CancellationToken ct = default)
        {
            await SeedRolesAsync(ct);
        }
    }
}