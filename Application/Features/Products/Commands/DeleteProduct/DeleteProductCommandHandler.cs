using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Domain.Entities;

namespace Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, Result>
    {
        private readonly IProductRepository _products;
        private readonly IItemRepository _items;
        private readonly ICurrentUser _currentUser;
        private readonly IAuditService _audit;

        public DeleteProductCommandHandler(
            IProductRepository products,
            IItemRepository items,
            ICurrentUser currentUser,
            IAuditService audit)
        {
            _products = products;
            _items = items;
            _currentUser = currentUser;
            _audit = audit;
        }

        public async Task<Result> Handle(DeleteProductCommand command, CancellationToken ct = default)
        {
            if (!_currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Only SuperAdmins can delete products");

            var product = await _products.GetByIdAsync(command.ProductId, ct);
            if (product is null)
                return Result.Failure("Invalid product");

            // Check if any items with IsSold=true exist for this product
            var hasSoldItems = await _items.HasSoldItemsByProductIdAsync(command.ProductId, ct);
            if (hasSoldItems)
                return Result.Failure("Cannot delete product. There are items that are sold for this product.");

            await _products.DeleteAsync(product, ct);

            await _audit.RecordAsync("DeleteProduct", nameof(Product), product.Id.ToString(), _currentUser.UserId, ct);

            return Result.Success();
        }
    }
}

