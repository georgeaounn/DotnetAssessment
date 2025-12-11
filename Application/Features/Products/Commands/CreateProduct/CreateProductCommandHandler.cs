
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Common;
using Application.Features.Products.Dtos;
using Domain.Entities;

namespace Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IProductRepository _products;
    private readonly ICurrentUser _currentUser;
    private readonly IAuditService _audit;

    public CreateProductCommandHandler(
        IProductRepository products,
        ICurrentUser currentUser,
        IAuditService audit)
    {
        _products = products;
        _currentUser = currentUser;
        _audit = audit;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand command, CancellationToken ct = default)
    {
        if (!_currentUser.IsSuperAdmin)
            throw new UnauthorizedAccessException("Only SuperAdmins can create products");

        bool NameExists = await _products.NameExistsAsync(command.Name, ct);
        if(NameExists)
            return Result<ProductDto>.Failure("A product with a similar name already exists");

        var product = new Product() { Name = command.Name, BasePrice = command.BasePrice };
        await _products.AddAsync(product, ct);

        await _audit.RecordAsync("CreateProduct", nameof(Product), product.Id.ToString(), _currentUser.UserId, ct);

        return Result<ProductDto>.Success(new ProductDto(product.Id, product.Name, product.BasePrice, product.IsActive));
    }
}