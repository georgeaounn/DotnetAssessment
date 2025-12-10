using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.CQRS
{
    public interface ICommandDispatcher
    {
        Task<TResult> Dispatch<TResult>(ICommand<TResult> command, CancellationToken ct = default);

    }
}
