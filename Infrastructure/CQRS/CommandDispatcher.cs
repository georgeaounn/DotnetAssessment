
using Application.Abstractions.CQRS;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CQRS
{

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _sp;

        public CommandDispatcher(IServiceProvider sp)
        {
            _sp = sp;
        }

        public async Task<TResult> Dispatch<TResult>(ICommand<TResult> command, CancellationToken ct = default)
        {
            // Validate command
            var validatorType = typeof(IValidator<>).MakeGenericType(command.GetType());
            var validator = _sp.GetService(validatorType);

            if (validator != null)
            {
                var validationContext = new ValidationContext<object>(command);
                var validationResult = await ((IValidator)validator).ValidateAsync(validationContext, ct);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            // Dispatch to handler
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic handler = _sp.GetRequiredService(handlerType);
            return await handler.Handle((dynamic)command, ct);
        }
    }
}