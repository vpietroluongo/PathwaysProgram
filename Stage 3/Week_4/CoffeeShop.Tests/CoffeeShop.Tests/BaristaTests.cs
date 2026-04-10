using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

[TestClass]
public class BaristaTests
{
    private Mock<ICoffeeMachine> _mockCoffeeMachine;
    private Barista _barista;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockCoffeeMachine = new Mock<ICoffeeMachine>();
        _barista = new Barista(_mockCoffeeMachine.Object);
    }

    [TestMethod]
    [DataRow("Latte", true, false, true)]
    public async Task TakeOrderAsync_MachineWorks_OrderSucceeds(string drink, bool hasMilk, bool hasSugar, bool expectedResult)
    {
        //Arrange
        // _mockCoffeeMachine
        //     .Setup(m => m.MakeCoffeeAsync(It.IsAny<CoffeeOrder>()))
        //     .ReturnsAsync(true);
        _mockCoffeeMachine
            .Setup(m => m.MakeCoffeeAsync(It.Is<CoffeeOrder>(o => o.Drink == drink && o.HasMilk == hasMilk && o.HasSugar == hasSugar)))
            .ReturnsAsync(expectedResult);
        
        //Act
        //bool result = await _barista.TakeOrderAsync("Latte", true, false);
        bool result = await _barista.TakeOrderAsync(drink, hasMilk, hasSugar);

        //Assert
        //Assert.IsTrue(result);
        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task TakeOrderAsync_MachineFails_OrderFails()
    {
        //Arrange
        _mockCoffeeMachine
            .Setup(m => m.MakeCoffeeAsync(It.IsAny<CoffeeOrder>()))
            .ReturnsAsync(false);
        
        //Act
        bool result = await _barista.TakeOrderAsync("Latte", true, false);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task TakeOrderAsync_MachineThrowsException_TestHandlesIt()
    {
        //Arrange
        _mockCoffeeMachine
            .Setup(m => m.MakeCoffeeAsync(It.IsAny<CoffeeOrder>()))
            .ThrowsAsync(new Exception("Coffee machine is broken"));
        
        //Act and Assert
        await Assert.ThrowsExactlyAsync<Exception>(() => _barista.TakeOrderAsync("Cappuccino", true, true));
        // bool result = await _barista.TakeOrderAsync("Latte", true, false);

        // //Assert
        // Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task TakeOrderAsync_RemoveMockSetup_ReturnsDefault()
    {  
        //Act
        bool result = await _barista.TakeOrderAsync("Latte", true, false);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task TakeOrderAsync_ItIs_OrderSucceeds()
    {
        //Arrange
        // _mockCoffeeMachine
        //     .Setup(m => m.MakeCoffeeAsync(It.IsAny<CoffeeOrder>()))
        //     .ReturnsAsync(true);
               
        _mockCoffeeMachine
            .Setup(m => m.MakeCoffeeAsync(It.Is<CoffeeOrder>(order => 
                order.Drink == "Latte" &&
                order.HasMilk == true &&
                order.HasSugar == false)))
            .ReturnsAsync(true);
        
        //Act
        bool result = await _barista.TakeOrderAsync("Latte", true, false);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CanServeCustomerAsync_HasEnoughStock_ReturnsTrue()
    {
        _mockCoffeeMachine
            .Setup(m => m.GetCoffeeStockAsync())
            .ReturnsAsync(12);
        
        bool result = await _barista.CanServeCustomersAsync();

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CanServeCustomerAsync_NotEnoughStock_ReturnsFalse()
    {
        _mockCoffeeMachine
            .Setup(m => m.GetCoffeeStockAsync())
            .ReturnsAsync(3);
        
        bool result = await _barista.CanServeCustomersAsync();

        Assert.IsFalse(result);
    }
}