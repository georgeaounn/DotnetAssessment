
using Application.Abstractions.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CQRS;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _sp;

    public CommandDispatcher(IServiceProvider sp)
    {
        _sp = sp;
    }

    public Task<TResult> Dispatch<TResult>(ICommand<TResult> command, CancellationToken ct = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        dynamic handler = _sp.GetRequiredService(handlerType);
        return handler.Handle((dynamic)command, ct);
    }
}

