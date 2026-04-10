using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

[TestClass]
public class RocketControllerTests
{
    private Mock<IRocketEngine> _mockEngine;
    private RocketController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockEngine = new Mock<IRocketEngine>();
        _controller = new RocketController(_mockEngine.Object);
    }

    [TestMethod]
    public async Task PrepareForLaunchAsync_AllSystemsGood_ReturnsTrue()
    {
        //Arrange
        _mockEngine
            .Setup(e => e.CheckEnginesAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        _mockEngine
            .Setup(e => e.GetFuelLevelAsync(It.IsAny<string>()))
            .ReturnsAsync(80);
        _mockEngine
            .Setup(e => e.LaunchAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        //Act
        bool launched = await _controller.PrepareForLaunchAsync("Falcon9");

        //Assert
        Assert.IsTrue(launched);
    }

    [TestMethod]
    [DataRow("Artemis", true, 90, true)]
    [DataRow("Apollo", true, 79, false)]
    [DataRow("Rocket", false, 80, false)]
    public async Task PrepareForLaunchAsync_EngineAndFuelTests_ReturnsExpectedResult(string rocketId, bool engineCheck, int fuelLevel, bool expected)
    {
        //Arrange
        _mockEngine
            .Setup(e => e.CheckEnginesAsync(It.IsAny<string>()))
            .ReturnsAsync(engineCheck);
        _mockEngine
            .Setup(e => e.GetFuelLevelAsync(It.IsAny<string>()))
            .ReturnsAsync(fuelLevel);
        _mockEngine
            .Setup(e => e.LaunchAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        //Act
        bool launched = await _controller.PrepareForLaunchAsync(rocketId);

        //Assert
        Assert.AreEqual(expected, launched);
    }

    [TestMethod]
    public async Task PrepareForLaunchAsync_LowFuel_ReturnsFalse()
    {
        //Arrange
        _mockEngine
            .Setup(e => e.CheckEnginesAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        _mockEngine
            .Setup(e => e.GetFuelLevelAsync(It.IsAny<string>()))
            .ReturnsAsync(15);
        _mockEngine
            .Setup(e => e.LaunchAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        
        //Act
        bool result = await _controller.PrepareForLaunchAsync("Artemis II");

        //Assert
        Assert.IsFalse(result);
        _mockEngine.Verify(e => e.LaunchAsync(It.IsAny<string>()), Times.Never);

    }

        [TestMethod]
    public async Task PrepareForLaunchAsync_LowFuel_WithoutLaunchMock_ReturnsFalse()
    {
        //Arrange
        _mockEngine
            .Setup(e => e.CheckEnginesAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        _mockEngine
            .Setup(e => e.GetFuelLevelAsync(It.IsAny<string>()))
            .ReturnsAsync(15);

        //Act
        bool result = await _controller.PrepareForLaunchAsync("Artemis II");

        //Assert
        Assert.IsFalse(result);
        _mockEngine.Verify(e => e.LaunchAsync(It.IsAny<string>()), Times.Never);

    }

    [TestMethod]
    public async Task PrepareForLaunchAsync_EngineCheckFails_ReturnsFalse()
    {
        //Arrange
        _mockEngine
            .Setup(e => e.CheckEnginesAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        
        //Act
        bool launched = await _controller.PrepareForLaunchAsync("Falcon9");

        //Assert
        Assert.IsFalse(launched);
        _mockEngine.Verify(e => e.GetFuelLevelAsync(It.IsAny<string>()), Times.Never);
        _mockEngine.Verify(e => e.LaunchAsync(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task PrepareForLaunchAsync_EngineThrowsException_TestFailsGracefully()
    {
        //Arrange
        _mockEngine
            .Setup(e => e.CheckEnginesAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Engine failed"));
        
        //Act and Assert
        await Assert.ThrowsExactlyAsync<Exception>(() => _controller.PrepareForLaunchAsync("Falcon9"));
    }

    [TestMethod]
    public async Task GetMissionStatusAsync_HighFuel_ReturnsGoForLaunch()
    {
        //Arrange
        _mockEngine
            .Setup(e => e.GetFuelLevelAsync(It.IsAny<string>()))
            .ReturnsAsync(51);
        
        //Act
        string status = await _controller.GetMissionStatusAsync("Falcon9");

        //Assert
        Assert.AreEqual("Go for launch!", status);

    }

    [TestMethod]
    public async Task GetMissionStatusAsync_LowFuel_ReturnsRefuelNeeded()
    {
        //Arrange
        _mockEngine
            .Setup(e => e.GetFuelLevelAsync(It.IsAny<string>()))
            .ReturnsAsync(49);
        
        //Act
        string status = await _controller.GetMissionStatusAsync("Falcon9");

        //Assert
        Assert.AreEqual("Refuel needed!", status);
    }

    [TestMethod]
    public async Task PrepareForLaunchAsync_VerifyLaunchIsCalled()
    {
        //Arrange
        _mockEngine
            .Setup(e => e.CheckEnginesAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        _mockEngine
            .Setup(e => e.GetFuelLevelAsync(It.IsAny<string>()))
            .ReturnsAsync(90);
        _mockEngine
            .Setup(e => e.LaunchAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        
        //Act
        bool launched = await _controller.PrepareForLaunchAsync("Falcon9");

        //Assert
        Assert.IsTrue(launched);
        _mockEngine.Verify(e => e.LaunchAsync(It.IsAny<string>()), Times.Once);

    }
}