using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Persistence
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id, CancellationToken ct);
    }
}
