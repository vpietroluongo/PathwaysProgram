using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

[TestClass]
public class CourseServiceTests
{
    private Mock<IEnrollmentRepository> _mockRepo;
    private Mock<INotificationService> _mockNotification;
    private CourseService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IEnrollmentRepository>();
        _mockNotification = new Mock<INotificationService>();
        _service = new CourseService(_mockRepo.Object, _mockNotification.Object);
    }

    // === Write and experiment with your tests here ===

    [TestMethod]
    public async Task ProcessRequestAsync_EnrollNewStudent_Succeeds()
    {
        // Arrange - TODO: Setup mocks for EnrollStudentRequest
        _mockRepo
            .Setup(r => r.GetEnrollmentAsync(101, 5))
            .ReturnsAsync((Enrollment?)null);
        _mockRepo
            .Setup(r => r.IsCourseFullAsync(5))
            .ReturnsAsync(false);
        _mockRepo
            .Setup(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()))
            .Returns(Task.CompletedTask);
        _mockNotification
            .Setup(n => n.SendEnrollmentConfirmationAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        var request = new EnrollStudentRequest(101, 5, "student@example.com", "C# Mastery");
        bool result = await _service.ProcessRequestAsync(request);

        // Assert
        Assert.IsTrue(result);
        _mockRepo.Verify(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()), Times.Once);
        _mockNotification.Verify(r => r.SendEnrollmentConfirmationAsync("student@example.com", "C# Mastery"), Times.Once);
    }

    // Add more tests below and try different Moq setups!
    [TestMethod]
    public async Task ProcessRequestAsync_StudentAlreadyEnrolled_ReturnsFalse()
    {
       // Arrange
        var existing = new Enrollment(1, 101, 5, DateTime.UtcNow);
        _mockRepo
            .Setup(r => r.GetEnrollmentAsync(101, 5))
            .ReturnsAsync(existing);

        // Act
        var request = new EnrollStudentRequest(101, 5, "student@example.com", "C# Mastery");
        bool result = await _service.ProcessRequestAsync(request);

        // Assert
        Assert.IsFalse(result);
        _mockRepo.Verify(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()), Times.Never);
        _mockNotification.Verify(r => r.SendEnrollmentConfirmationAsync("student@example.com", "C# Mastery"), Times.Never);
    }

    [TestMethod]
    public async Task ProcessRequestAsync_CourseIsFull_SendsNotificationAndReturnsFalse()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetEnrollmentAsync(101, 5))
            .ReturnsAsync((Enrollment?)null);
        _mockRepo
            .Setup(r => r.IsCourseFullAsync(5))
            .ReturnsAsync(true);
        _mockNotification
            .Setup(n => n.SendCourseFullNotificationAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        var request = new EnrollStudentRequest(101, 5, "student@example.com", "C# Mastery");
        bool result = await _service.ProcessRequestAsync(request);

        // Assert
        Assert.IsFalse(result);
        _mockRepo.Verify(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()), Times.Never);
        _mockNotification.Verify(r => r.SendEnrollmentConfirmationAsync("student@example.com", "C# Mastery"), Times.Never);
        _mockNotification.Verify(r => r.SendCourseFullNotificationAsync("C# Mastery"), Times.Once);
    }


    [TestMethod]
    public async Task ProcessRequestAsync_CheckAvailability_ReturnsCorrectResult()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetEnrollmentCountAsync(101))
            .ReturnsAsync(17);
        
        //Act
        var request = new CheckAvailabilityRequest(101);
        var available = await _service.ProcessRequestAsync(request);

        //Assert
        Assert.IsTrue(available);
    }

    [TestMethod]
    public async Task ProcessRequestAsync_CheckAvailability_ReturnsFalse()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetEnrollmentCountAsync(101))
            .ReturnsAsync(30);
        
        //Act
        var request = new CheckAvailabilityRequest(101);
        var available = await _service.ProcessRequestAsync(request);

        //Assert
        Assert.IsFalse(available);
    }

    [TestMethod]
    public async Task ProcessRequestAsync_UnsupportedRequest_ThrowsException()
    {
        //Arrange
        var invalidRequest = new CheckAvailabilityRequest(999);

        //Act and Assert
        await Assert.ThrowsExactlyAsync<ArgumentException>(
            () => _service.ProcessRequestAsync(new CourseRequest(0, 0)));  //removed abstract from CourseRecord class to get this to work
    }
}