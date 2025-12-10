using Application.Abstractions.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CQRS
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _sp;

        public QueryDispatcher(IServiceProvider sp)
        {
            _sp = sp;
        }

        public Task<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken ct = default)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _sp.GetRequiredService(handlerType);
            return handler.Handle((dynamic)query, ct);
        }
    }

}