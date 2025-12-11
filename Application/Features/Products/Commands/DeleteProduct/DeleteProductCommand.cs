using Application.Abstractions.CQRS;
using Application.Common;

namespace Application.Features.Products.Commands.DeleteProduct
{
    public record DeleteProductCommand(Guid ProductId) : ICommand<Result>;
}

