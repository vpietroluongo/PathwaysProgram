using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Runtime.CompilerServices;
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

    // === Write your tests here ===

    [TestMethod]
    public async Task EnrollStudentAsync_NewStudent_SuccessfullyEnrolls()
    {
        // Arrange - TODO
        // _mockRepo
        //     .Setup(r => r.GetEnrollmentAsync(It.IsAny<int>(), It.IsAny<int>()))
        //     .ReturnsAsync((Enrollment?)null);
        // _mockRepo   
        //     .Setup(r => r.IsCourseFullAsync(It.IsAny<int>()))
        //     .ReturnsAsync(false);
        // _mockRepo
        //     .Setup(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()))
        //     .Returns(Task.CompletedTask);
        // _mockNotification
        //     .Setup(n => n.SendEnrollmentConfirmationAsync(It.IsAny<string>(), It.IsAny<string>()))
        //     .Returns(Task.CompletedTask);
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
        bool result = await _service.EnrollStudentAsync(101, 5, "student@example.com", "C# Basics");

        // Assert
        Assert.IsTrue(result);
        _mockNotification.Verify(n => n.SendCourseFullNotificationAsync(It.IsAny<string>()), Times.Never);
        _mockRepo.Verify(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()), Times.Once);
        _mockNotification.Verify(n => n.SendEnrollmentConfirmationAsync("student@example.com", "C# Basics"), Times.Once);
    }   
    
    [TestMethod]
    public async Task EnrollStudentAsync_AlreadyEnrolled_ReturnsFalse()
    {
       _mockRepo
            .Setup(r => r.GetEnrollmentAsync(101, 5))
            .ReturnsAsync(new Enrollment
            {
                Id = 999,
                StudentId = 101,
                CourseId = 5,
                IsCompleted = false
            });
        _mockRepo   
            .Setup(r => r.IsCourseFullAsync(5))
            .ReturnsAsync(false);
        // _mockRepo
        //     .Setup(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()))
        //     .Returns(Task.CompletedTask);
        // _mockNotification
        //     .Setup(n => n.SendEnrollmentConfirmationAsync(It.IsAny<string>(), It.IsAny<string>()))
        //     .Returns(Task.CompletedTask);

        // Act
        bool result = await _service.EnrollStudentAsync(101, 5, "student@example.com", "C# Basics");

        // Assert
        Assert.IsFalse(result);
        _mockNotification.Verify(n => n.SendCourseFullNotificationAsync(It.IsAny<string>()), Times.Never);
        _mockRepo.Verify(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()), Times.Never);
        _mockNotification.Verify(n => n.SendEnrollmentConfirmationAsync("student@example.com", "C# Basics"), Times.Never);
        _mockRepo.Verify(n => n.IsCourseFullAsync(It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public async Task EnrollStudentAsync_CourseIsFull_ReturnsFalseAndSendsFullNotification()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetEnrollmentAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(It.IsAny<Enrollment>());
        _mockRepo
            .Setup(r => r.IsCourseFullAsync(101))
            .ReturnsAsync(true);
        _mockNotification
            .Setup(n => n.SendCourseFullNotificationAsync("Pathways"))
            .Returns(Task.CompletedTask);

        //Act
        bool result = await _service.EnrollStudentAsync(35, 101, "student@example.com", "Pathways");

        //Assert
        Assert.IsFalse(result);
        _mockRepo.Verify(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()), Times.Never);
        _mockNotification.Verify(n => n.SendEnrollmentConfirmationAsync("student@example.com", "Pathways"), Times.Never);
        _mockNotification.Verify(n => n.SendCourseFullNotificationAsync("Pathways"), Times.Once);
    }

    [TestMethod]
    public async Task EnrollStudentAsync_CourseIsNotFull_ReturnsTrueAndSendsConfirmationNotification()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetEnrollmentAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(It.IsAny<Enrollment>());
        _mockRepo
            .Setup(r => r.IsCourseFullAsync(It.Is<int>(courseId => courseId == 101)))
            .ReturnsAsync(false);
        _mockNotification
            .Setup(n => n.SendCourseFullNotificationAsync("Pathways"))
            .Returns(Task.CompletedTask);

        //Act
        bool result = await _service.EnrollStudentAsync(35, 101, "student@example.com", "Pathways");

        //Assert
        Assert.IsTrue(result);
        _mockRepo.Verify(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()), Times.Once);
        _mockNotification.Verify(n => n.SendEnrollmentConfirmationAsync("student@example.com", "Pathways"), Times.Once);
        _mockNotification.Verify(n => n.SendCourseFullNotificationAsync("Pathways"), Times.Never);
    }

    [TestMethod]
    public async Task EnrollStudentAsync_ItIsExample_ReturnsTrueAndSendsConfirmationNotification()
    {
        //Arrange
                var enrollment = new Enrollment
        {
            StudentId = 35,
            CourseId = 101,
            EnrolledDate = DateTime.UtcNow,
            IsCompleted = false
        };
        _mockRepo
            .Setup(r => r.GetEnrollmentAsync(35, 101))
            .ReturnsAsync(It.IsAny<Enrollment>());
        _mockRepo
            .Setup(r => r.IsCourseFullAsync(101))
            .ReturnsAsync(false);
        _mockNotification
            .Setup(n => n.SendCourseFullNotificationAsync("Pathways"))
            .Returns(Task.CompletedTask);

        //Act
        bool result = await _service.EnrollStudentAsync(35, 101, "student@example.com", "Pathways");

        //Assert
        Assert.IsTrue(result);
        _mockRepo.Verify(r => r.GetEnrollmentAsync(It.Is<int>(studentId => studentId == 35), It.Is<int>(courseId => courseId == 101)), Times.Once);
        _mockRepo.Verify(r => r.IsCourseFullAsync(It.Is<int>(courseId => courseId == 101)), Times.Once);
        _mockRepo.Verify(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()), Times.Once);
        _mockNotification.Verify(n => n.SendEnrollmentConfirmationAsync("student@example.com", "Pathways"), Times.Once);
        _mockNotification.Verify(n => n.SendCourseFullNotificationAsync("Pathways"), Times.Never);
    }

    [TestMethod]
    public async Task GetAvailableSpotsAsync_ReturnsCorrectCount()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetEnrollmentCountAsync(101))
            .ReturnsAsync(23);

        //Act
        int available = await _service.GetAvailableSpotsAsync(101);

        //Assert
        Assert.AreEqual(available, 7);
        _mockRepo.Verify(r => r.GetEnrollmentCountAsync(It.Is<int>(courseId => courseId == 101)), Times.Once);
    }

    [TestMethod]
    public async Task EnrollStudentAsync_RepositoryThrowsException_PropagatesException()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetEnrollmentAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("Database error!"));

        //Act and Assert
        await Assert.ThrowsExactlyAsync<Exception>(() => _service.EnrollStudentAsync(25, 101, "student@example.com", "Pathways"));
    }

    [TestMethod]
    public async Task EnrollStudentAsync_AddEnrollmentThrowsException_PropagatesException()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetEnrollmentAsync(25, 101))
            .ReturnsAsync((Enrollment?)null);
        _mockRepo   
            .Setup(r => r.IsCourseFullAsync(101))
            .ReturnsAsync(false);
        _mockRepo
            .Setup(r => r.AddEnrollmentAsync(It.IsAny<Enrollment>()))
            .Throws(new Exception("Database error!"));

        //Act and Assert
        await Assert.ThrowsExactlyAsync<Exception>(() => _service.EnrollStudentAsync(25, 101, "student@example.com", "Pathways"));
    }
}
