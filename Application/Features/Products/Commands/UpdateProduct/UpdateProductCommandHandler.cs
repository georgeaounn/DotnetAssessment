using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Products.Dtos;
using Domain.Entities;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Result<ProductDto>>
    {
        private readonly IProductRepository _products;
        private readonly ICurrentUser _currentUser;
        private readonly IAuditService _audit;

        public UpdateProductCommandHandler(IProductRepository products, ICurrentUser currentUser, IAuditService audit)
        {
            _products = products;
            _currentUser = currentUser;
            _audit = audit;
        }

        public async Task<Result<ProductDto>> Handle(UpdateProductCommand command, CancellationToken ct = default)
        {
            if(!_currentUser.IsSuperAdmin)
                throw new UnauthorizedAccessException("Only SuperAdmins can update products");

            var product = await _products.GetByIdAsync(command.ProductId, ct);
            if(product is null)
                return Result<ProductDto>.Failure("Product not found");

            // Check if name already exists for a different product
            var nameExists = await _products.NameExistsAsync(command.Name, ct);
            if(nameExists && product.Name != command.Name)
                return Result<ProductDto>.Failure("A product with this name already exists");

            product.Name = command.Name;
            product.BasePrice = command.BasePrice;
            product.IsActive = command.IsActive;

            await _products.UpdateAsync(product, ct);

            await _audit.RecordAsync("UpdateProduct", nameof(Product), product.Id.ToString(), _currentUser.UserId, ct);

            return Result<ProductDto>.Success(
                new ProductDto(product.Id, product.Name, product.BasePrice, product.IsActive));
        }
    }
}

