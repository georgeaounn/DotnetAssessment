using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Persistence
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken ct);
        Task AddAsync(User user, CancellationToken ct);
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    }
}
