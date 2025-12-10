using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        int RoleId { get; }
        bool IsSuperAdmin => RoleId == 1;
    }
}
