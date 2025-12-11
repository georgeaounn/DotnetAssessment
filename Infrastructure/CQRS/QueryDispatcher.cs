using Application.Abstractions.CQRS;
using FluentValidation;
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

        public async Task<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken ct = default)
        {
            // Validate query
            var validatorType = typeof(IValidator<>).MakeGenericType(query.GetType());
            var validator = _sp.GetService(validatorType);
            
            if (validator != null)
            {
                var validationContext = new ValidationContext<object>(query);
                var validationResult = await ((IValidator)validator).ValidateAsync(validationContext, ct);
                
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            // Dispatch to handler
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _sp.GetRequiredService(handlerType);
            return await handler.Handle((dynamic)query, ct);
        }
    }

}