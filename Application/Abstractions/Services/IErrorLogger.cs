using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IErrorLogger
    {
        Task LogErrorAsync(Exception ex, string? context, CancellationToken ct = default);

    }
}
