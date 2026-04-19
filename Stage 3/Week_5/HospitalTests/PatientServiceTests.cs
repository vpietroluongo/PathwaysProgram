using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading.Tasks;

[TestClass]
public class PatientServiceTests
{
    private Mock<IPatientRepository> _mockRepo;
    private Mock<INotificationService> _mockNotification;
    private PatientService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IPatientRepository>();
        _mockNotification = new Mock<INotificationService>();
        _service = new PatientService(_mockRepo.Object, _mockNotification.Object);
    }

    // === Write your tests and experiment with different Moq techniques ===

    [TestMethod]
    public async Task AdmitPatientAsync_CreatesPatientAndSendsNotification()
    {
        // Arrange
        _mockRepo
            .Setup(r => r.AddPatientAsync(It.IsAny<Patient>()))
            .Returns(Task.CompletedTask);
        _mockNotification
            .Setup(n => n.SendAdmissionNotificationAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
 
        // Act
        int patientId = await _service.AdmitPatientAsync("John Doe", "john@example.com", "555-1234", new DateTime(1990, 5, 15));

        // Assert
        //Assert.IsTrue(patientId > 0);
        _mockRepo.Verify(r => r.AddPatientAsync(It.Is<Patient>(p =>
            p.FullName == "John Doe" &&
            p.Email == "john@example.com" &&
            p.Phone == "555-1234" &&
            p.DateOfBirth == new DateTime(1990, 5, 15) &&
            p.IsAdmitted
            )), Times.Once);
        _mockNotification.Verify(n => n.SendAdmissionNotificationAsync("john@example.com", "John Doe"), Times.Once);
    }

    [TestMethod]
    public async Task DischargePatientAsync_AdmittedPatient_SucceedsAndSendsNotification()
    {
        //Arrange
        var patient = new Patient
        {
            Id =123, 
            FullName = "John Doe", 
            Email = "john@example.com", 
            Phone = "555-1234", 
            DateOfBirth = new DateTime(1990, 5, 15), 
            IsAdmitted = true
        };
        _mockRepo
            .Setup(r => r.GetPatientAsync(It.IsAny<int>()))
            .ReturnsAsync(patient);
        _mockRepo
            .Setup(r => r.UpdatePatientAsync(patient))
            .Returns(Task.CompletedTask);
        _mockNotification
            .Setup(n => n.SendDischargeNotificationAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        //Act
        await _service.DischargePatientAsync(123);

        //Assert
        Assert.IsFalse(patient.IsAdmitted);
        _mockRepo.Verify(r => r.GetPatientAsync(123), Times.Once);
        _mockNotification.Verify(n => n.SendDischargeNotificationAsync("john@example.com", "John Doe"), Times.Once);
    }

    [TestMethod]
    public async Task DischargePatientAsync_PatientNotFound_ThrowsException()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetPatientAsync(123))
            .ReturnsAsync((Patient?)null);
        
        //Act and Assert
        Assert.ThrowsExactlyAsync<InvalidOperationException>(() => _service.DischargePatientAsync(123));

    }

    [TestMethod]
    public async Task GetCurrentlyAdmittedPatientsAsync_ReturnsOnlyAdmittedPatients()
    {
        //Arrange
        var patients = new List<Patient>()
        {
            new Patient
            {            
                Id =123, 
                FullName = "John Doe", 
                Email = "john@example.com", 
                Phone = "555-1234", 
                DateOfBirth = new DateTime(1990, 5, 15), 
                IsAdmitted = true 
            },
            new Patient
            {
                Id =456, 
                FullName = "Jane Doe", 
                Email = "jane@example.com", 
                Phone = "555-4321", 
                DateOfBirth = new DateTime(1970, 8, 26), 
                IsAdmitted = false
            },
            new Patient
            {
                Id =789, 
                FullName = "Alice Alice", 
                Email = "alice@example.com", 
                Phone = "555-0000", 
                DateOfBirth = new DateTime(2000, 1, 1), 
                IsAdmitted = true
            },
            new Patient
            {
                Id =374, 
                FullName = "Bob Bob", 
                Email = "bob@example.com", 
                Phone = "555-4321", 
                DateOfBirth = new DateTime(1967, 10, 18), 
                IsAdmitted = false
            }
        };
        _mockRepo
            .Setup(r => r.GetAdmittedPatientsAsync())
            .ReturnsAsync(patients);

        //Act
        var admittedPatients = await _service.GetCurrentlyAdmittedPatientsAsync();

        //Assert
        Assert.AreEqual(2, admittedPatients.Count());
        Assert.IsTrue(admittedPatients.Any(p => p.IsAdmitted));
        Assert.IsTrue(admittedPatients.Any(p => p.Id == 789));
        Assert.IsTrue(admittedPatients.Any(p => p.Id == 123));
    }

    [TestMethod]
    public async Task SendEmergencyAlertAsync_PatientExists_SendsCriticalAlert()
    {
        //Arrange
        var patient = new Patient
        {
            Id =123, 
            FullName = "John Doe", 
            Email = "john@example.com", 
            Phone = "555-1234", 
            DateOfBirth = new DateTime(1990, 5, 15), 
            IsAdmitted = true
        };
        _mockRepo
            .Setup(r => r.GetPatientAsync(123))
            .ReturnsAsync(patient);
        _mockNotification
            .Setup(n => n.SendCriticalAlertAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        //Act
        await _service.SendEmergencyAlertAsync(123, "Critical alert!!!!");

        //Assert
        _mockNotification.Verify(n => n.SendCriticalAlertAsync("555-1234", "Critical alert!!!!"), Times.Once);

    }

        [TestMethod]
    public async Task SendEmergencyAlertAsync_PatientDoesNotExists_ThrowsException()
    {
        //Arrange

        _mockRepo
            .Setup(r => r.GetPatientAsync(123))
            .ReturnsAsync((Patient?)null);

        //Act and Assert
        Assert.ThrowsExactlyAsync<InvalidOperationException>(() => _service.SendEmergencyAlertAsync(123, "Critical alert!"));
        _mockNotification.Verify(n => n.SendCriticalAlertAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}