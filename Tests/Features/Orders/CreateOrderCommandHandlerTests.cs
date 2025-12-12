using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Dtos;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Features.Orders;

public class CreateOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenItemsListIsEmpty_ReturnsFailure()
    {
        var itemsRepo = new Mock<IItemRepository>();
        var ordersRepo = new Mock<IOrderRepository>();
        var auditService = new Mock<IAuditService>();
        var handler = new CreateOrderCommandHandler(itemsRepo.Object, ordersRepo.Object, auditService.Object);
        
        var command = new CreateOrderCommand(Guid.NewGuid(), new List<OrderItemRequest>());

        var result = await handler.Handle(command);

        Assert.True(result.IsFailure);
        Assert.Equal("Order must contain at least one item", result.Error);
    }

    [Fact]
    public async Task Handle_WhenItemsNotFound_ReturnsFailure()
    {
        var itemsRepo = new Mock<IItemRepository>();
        var ordersRepo = new Mock<IOrderRepository>();
        var auditService = new Mock<IAuditService>();
        var handler = new CreateOrderCommandHandler(itemsRepo.Object, ordersRepo.Object, auditService.Object);
        
        var itemId = Guid.NewGuid();
        var command = new CreateOrderCommand(Guid.NewGuid(), new List<OrderItemRequest> { new(itemId) });
        
        itemsRepo.Setup(x => x.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Item>()); // Return empty list - items not found

        var result = await handler.Handle(command);

        Assert.True(result.IsFailure);
        Assert.Equal("Some items not found", result.Error);
    }

    [Fact]
    public async Task Handle_WhenValidItems_ReturnsSuccess()
    {
        var itemsRepo = new Mock<IItemRepository>();
        var ordersRepo = new Mock<IOrderRepository>();
        var auditService = new Mock<IAuditService>();
        var handler = new CreateOrderCommandHandler(itemsRepo.Object, ordersRepo.Object, auditService.Object);
        
        var customerId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new CreateOrderCommand(customerId, new List<OrderItemRequest> { new(itemId) });
        
        var product = new Product { Id = productId, Name = "Test Product", BasePrice = 100 };
        var item = new Item 
        { 
            Id = itemId, 
            Name = "Test Item", 
            ProductId = productId,
            Product = product
        };
        
        itemsRepo.Setup(x => x.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Item> { item });
        
        ordersRepo.Setup(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        itemsRepo.Setup(x => x.UpdateItemStatus(It.IsAny<List<Guid>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        auditService.Setup(x => x.RecordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await handler.Handle(command);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(customerId, result.Data.CustomerId);
        Assert.Single(result.Data.Items);
        Assert.Equal(100, result.Data.TotalPrice);
        
        ordersRepo.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        itemsRepo.Verify(x => x.UpdateItemStatus(It.IsAny<List<Guid>>(), true, It.IsAny<CancellationToken>()), Times.Once);
    }
}

