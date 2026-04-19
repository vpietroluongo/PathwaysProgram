using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data;
using System.Threading.Tasks;

[TestClass]
public class RideServiceTests
{
    private Mock<IRideRepository> _mockRepo;
    private Mock<INotificationService> _mockNotification;
    private RideService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IRideRepository>();
        _mockNotification = new Mock<INotificationService>();
        _service = new RideService(_mockRepo.Object, _mockNotification.Object);
    }

    // === Write and experiment with your tests here ===

    [TestMethod]
    public async Task ProcessRequestAsync_BookRide_SuccessfullyBooksRide()
    {
        // Arrange 
        var driver = new Driver(1234, "Bob", "Honda", 5, true);
        _mockRepo
            .Setup(r => r.FindAvailableDriverAsync("123 Main St"))
            .ReturnsAsync(driver);
        _mockRepo
            .Setup(r => r.AddRideAsync(It.IsAny<Ride>()))
            .Returns(Task.CompletedTask);
        _mockNotification
            .Setup(n => n.SendRideConfirmationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        _mockNotification
            .Setup(n => n.SendDriverAssignedNotificationAsync(It.IsAny<string>(), It.Is<string>(a => a == "123 Main St"), It.Is<string>(a => a == "456 Airport Blvd")))
            .Returns(Task.CompletedTask);

        var request = new BookRideRequest(42, "123 Main St", "456 Airport Blvd", 25.50m);

        // Act
        var result = await _service.ProcessRequestAsync(request);

        // Assert
        Assert.IsTrue((bool)result);
        _mockRepo.Verify(r => r.AddRideAsync(It.IsAny<Ride>()), Times.Once);
        _mockNotification.Verify(n => n.SendRideConfirmationAsync(It.IsAny<string>(), "123 Main St", "456 Airport Blvd"), Times.Once);
        _mockNotification.Verify(n => n.SendDriverAssignedNotificationAsync(It.IsAny<string>(), "Bob", "Honda"), Times.Once);
        _mockNotification.Verify(n => n.SendRideCancelledNotificationAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task ProcessRequestAsync_NoAvailableDriver_ReturnsFalse()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.FindAvailableDriverAsync(It.IsAny<string>()))
            .ReturnsAsync((Driver?)null);

        //Act
        var request = new BookRideRequest(42, "123 Main St", "456 Airport Blvd", 25.50m);
        var result = await _service.ProcessRequestAsync(request);

        //Assert
        Assert.IsFalse((bool)result);
        _mockRepo.Verify(r => r.AddRideAsync(It.IsAny<Ride>()), Times.Never);
    }

        [TestMethod]
    public async Task ProcessRequestAsync_ADriverIsUnavailable_ReturnsFalse()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.FindAvailableDriverAsync(It.IsAny<string>()))
            .ReturnsAsync(new Driver(1234, "Bob", "Honda", 5, false));

        //Act
        var request = new BookRideRequest(42, "123 Main St", "456 Airport Blvd", 25.50m);
        var result = await _service.ProcessRequestAsync(request);

        //Assert
        Assert.IsFalse((bool)result);
        _mockRepo.Verify(r => r.AddRideAsync(It.IsAny<Ride>()), Times.Never);
    }

    [TestMethod]
    public async Task ProcessRequestAsync_CancelRide_SuccessfullyCancels()
    {
        //Arrange
        var ride = new Ride(500, 1234, 4321, "123 Main Street", "456 Airport Blvd", 
                   25.50m, DateTime.UtcNow);
        _mockRepo
            .Setup(r => r.GetRideAsync(500))
            .ReturnsAsync(ride);
        _mockRepo
            .Setup(r => r.UpdateRideAsync(It.IsAny<Ride>()))
            .Returns(Task.CompletedTask);
        _mockNotification
            .Setup(n => n.SendRideCancelledNotificationAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        //Act
        var request = new CancelRideRequest(42, 500, "No longer needed");
        var result = await _service.ProcessRequestAsync(request);

        //Assert
        Assert.IsTrue((bool)result);
        _mockRepo.Verify(r => r.UpdateRideAsync(It.IsAny<Ride>()), Times.Once);
        _mockNotification.Verify(n => n.SendRideCancelledNotificationAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task ProcessRequestAsync_CancelRide_RideNotFound_ReturnsFalse()
    {
        //Arrange
        var ride = new Ride(500, 1234, 4321, "123 Main Street", "456 Airport Blvd", 
                   25.50m, DateTime.UtcNow);

        //Act
        var request = new CancelRideRequest(42, 500, "No longer needed");
        var result = await _service.ProcessRequestAsync(request);

        //Assert
        Assert.IsFalse((bool)result);
        _mockRepo.Verify(r => r.UpdateRideAsync(It.IsAny<Ride>()), Times.Never);
        _mockNotification.Verify(n => n.SendRideCancelledNotificationAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task ProcessRequestAsync_CancelRide_RideAlreadyCompleted_ReturnsFalse()
    {
        //Arrange
        var ride = new Ride(500, 1234, 4321, "123 Main Street", "456 Airport Blvd", 
                   25.50m, DateTime.UtcNow, true);
        _mockRepo
            .Setup(r => r.GetRideAsync(500))
            .ReturnsAsync(ride);

        //Act
        var request = new CancelRideRequest(42, 500, "No longer needed");
        var result = await _service.ProcessRequestAsync(request);

        //Assert
        Assert.IsFalse((bool)result);
        _mockRepo.Verify(r => r.UpdateRideAsync(It.IsAny<Ride>()), Times.Never);
        _mockNotification.Verify(n => n.SendRideCancelledNotificationAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    [DataRow(true, "Completed")]
    [DataRow(false, "In Progress")]
    public async Task ProcessRequestAsync_GetRideStatus_ReturnsCorrectStatus(bool isCompleted, string expectedResult)
    {
        //Arrange
        var ride = new Ride(500, 1234, 4321, "123 Main Street", "456 Airport Blvd", 
                   25.50m, DateTime.UtcNow, isCompleted);
        _mockRepo
            .Setup(r => r.GetRideAsync(It.IsAny<int>()))
            .ReturnsAsync(ride);
        //Act
        var request = new GetRideStatusRequest(1234, 500);
        var result = await _service.ProcessRequestAsync(request);

        //Assert
        Assert.AreEqual(expectedResult, (string)result);
        _mockRepo.Verify(r => r.GetRideAsync(500), Times.Once);
    }

    [TestMethod]
    public async Task ProcessRequestAsync_UnsupportedRequest_ThrowsException()
    {
        //Arrange
        var request = new RideRequest(500);
        //Act and Assert
        await Assert.ThrowsExactlyAsync<ArgumentException>(() => _service.ProcessRequestAsync(request));

    }
}
