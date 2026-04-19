using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[TestClass]
public class OrderServiceTests
{
    private Mock<IOrderRepository> _mockRepo;
    private Mock<IKitchenService> _mockKitchen;
    private Mock<INotificationService> _mockNotification;
    private OrderService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IOrderRepository>();
        _mockKitchen = new Mock<IKitchenService>();
        _mockNotification = new Mock<INotificationService>();
        _service = new OrderService(_mockRepo.Object, _mockKitchen.Object, _mockNotification.Object);
    }

    // === Experiment with different Moq setups here ===

    [TestMethod]
    public async Task PlaceOrderAsync_ValidOrder_SucceedsAndSendsConfirmation()
    {
        // Arrange
        _mockRepo
            .Setup(r => r.AddOrderAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);
        _mockKitchen
            .Setup(k => k.CanPrepareOrderAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(true);
        _mockNotification
            .Setup(n => n.SendOrderConfirmationAsync(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        // Act
        int orderId = await _service.PlaceOrderAsync("Alice", "555-1111", new List<string> { "Burger", "Fries" }, 12.99m);

        // Assert
        //Assert.IsTrue(orderId > 0);
        _mockRepo.Verify(r => r.AddOrderAsync(It.Is<Order>(o => o.CustomerName == "Alice" && o.TotalAmount == 12.99m)), Times.Once);
        _mockKitchen.Verify(k => k.CanPrepareOrderAsync(It.Is<List<string>>(items => items.Contains("Burger") && items.Contains("Fries"))), Times.Once);
        _mockNotification.Verify(n => n.SendOrderConfirmationAsync("555-1111", It.IsAny<int>()), Times.Once);
    }

    [TestMethod]
    public async Task PlaceOrderAsync_KitchenCannotPrepare_ThrowsException()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.AddOrderAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);
        _mockKitchen
            .Setup(k => k.CanPrepareOrderAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(false);

        //Act and Assert
        var error = await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => _service.PlaceOrderAsync("Alice", "555-1111", new List<string> { "Burger", "Fries" }, 12.99m));
        Assert.AreEqual("Kitchen cannot prepare this order at the moment", error.Message);
        _mockNotification.Verify(n => n.SendOrderConfirmationAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public async Task MarkOrderReadyAsync_ValidOrder_UpdatesAndNotifies()
    {
        //Arrange
        var order = new Order
        {
            Id = 356, 
            CustomerName = "Alice", 
            PhoneNumber = "555-1111",
            Items = {"Burger", "Fries"},
            TotalAmount = 12.99m,
            IsPrepared = false
        };
        _mockRepo
            .Setup(r => r.GetOrderAsync(It.IsAny<int>()))
            .ReturnsAsync(order);
        _mockKitchen
            .Setup(k => k.MarkOrderAsPreparedAsync(It.IsAny<int>()))
            .Returns(Task.CompletedTask);
        _mockRepo
            .Setup(r => r.UpdateOrderAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);
        _mockNotification
            .Setup(n => n.SendReadyForPickupAsync(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        //Act
        await _service.MarkOrderReadyAsync(356);

        //Assert
        Assert.IsTrue(order.IsPrepared);
        _mockRepo.Verify(r => r.GetOrderAsync(356), Times.Once);
        _mockKitchen.Verify(k => k.MarkOrderAsPreparedAsync(356), Times.Once);
        _mockRepo.Verify(r => r.UpdateOrderAsync(order), Times.Once);
        _mockNotification.Verify(n => n.SendReadyForPickupAsync("555-1111", 356), Times.Once);
    }

    [TestMethod]
    public async Task MarkOrderReadyAsync_OrderNotFound_ThrowsException()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetOrderAsync(356))
            .ReturnsAsync((Order?)null);
        
        //Act and Assert
        var error = await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => _service.MarkOrderReadyAsync(356));
        Assert.AreEqual("Order not found", error.Message);
        _mockKitchen.Verify(k => k.MarkOrderAsPreparedAsync(356), Times.Never);
    }

    [TestMethod]
    public async Task GetPendingOrdersAsync_ReturnsOnlyNotPreparedOrders()
    {
        //Arrange
        var orders = new List<Order>()
        {
            new Order{Id = 1, CustomerName = "Alice", PhoneNumber = "555-1111", IsPrepared = true },
            new Order{Id = 2, CustomerName = "Bob", PhoneNumber = "555-2222", IsPrepared = false },
            new Order{Id = 3, CustomerName = "Charlie", PhoneNumber = "555-3333", IsPrepared = true },
            new Order{Id = 4, CustomerName = "Dan", PhoneNumber = "555-4444", IsPrepared = false },
            new Order{Id = 5, CustomerName = "Evie", PhoneNumber = "555-5555", IsPrepared = false },
        };
        _mockRepo
            .Setup(r => r.GetPendingOrdersAsync())
            .ReturnsAsync(orders);

        //Act
        var pendingOrders = await _service.GetPendingOrdersAsync();

        //Assert
        Assert.AreEqual(3, pendingOrders.Count());
        Assert.IsTrue(pendingOrders.All(o => !o.IsPrepared));
        Assert.IsTrue(pendingOrders.Any(o => o.CustomerName == "Bob"));
        Assert.IsTrue(pendingOrders.Any(o => o.CustomerName == "Dan"));
        Assert.IsTrue(pendingOrders.Any(o => o.CustomerName == "Evie"));
    }
}