using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

[TestClass]
public class TaskServiceTests
{
    private Mock<ITaskRepository> _mockRepo;
    private Mock<IEmailService> _mockEmail;
    private TaskService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<ITaskRepository>();
        _mockEmail = new Mock<IEmailService>();
        _service = new TaskService(_mockRepo.Object, _mockEmail.Object);
    }

    // === Write your tests and experiment with Moq setups below ===

    [TestMethod]
    public async Task CreateTaskAsync_CreatesTaskAndSendsNotification()
    {
        // Arrange
        _mockRepo
            .Setup(r => r.AddTaskAsync(It.Is<TaskItem>(i => i.Id == 356)))
            .Returns(Task.CompletedTask);
        _mockEmail
            .Setup(e => e.SendTaskAssignedNotificationAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        int taskId = await _service.CreateTaskAsync("Finish Report", "Q4 financial report", "john@example.com");

        // Assert
        //Assert.IsTrue(taskId > 0);
        _mockRepo.Verify(r => r.AddTaskAsync(It.Is<TaskItem>(i => i.Title == "Finish Report" && 
            i.Description == "Q4 financial report" &&
            i.AssignedTo == "john@example.com")), 
            Times.Once);
        _mockEmail.Verify(e => e.SendTaskAssignedNotificationAsync("john@example.com", "Finish Report"), Times.Once);
    }

    [TestMethod]
    public async Task CompleteTaskAsync_ExistingTask_MarksAsCompletedAndSendsEmail()
    {   
        //Arrange
        var task = new TaskItem
        {
            Id = 356, 
            Title ="Finish Report", 
            Description = "Q4 financial report", 
            AssignedTo = "john@example.com", 
            IsCompleted = false, 
            CreatedDate = DateTime.UtcNow.AddDays(-5), 
            DueDate =DateTime.UtcNow 
        };
        _mockRepo
            .Setup(r => r.GetTaskAsync(It.IsAny<int>()))
            .ReturnsAsync(task);
        _mockRepo
            .Setup(r => r.UpdateTaskAsync(task))
            .Returns(Task.CompletedTask);
        _mockEmail
            .Setup(e => e.SendTaskCompletedNotificationAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        //Act
        await _service.CompleteTaskAsync(356, "john@example.com");

        //Assert
        Assert.IsTrue(task.IsCompleted);
        _mockRepo.Verify(r => r.GetTaskAsync(It.Is<int>(t => t == 356)), Times.Once);
        _mockRepo.Verify(r => r.UpdateTaskAsync(task), Times.Once);
        _mockEmail.Verify(e => e.SendTaskCompletedNotificationAsync("john@example.com", "Finish Report"));

    }

    [TestMethod]
    public async Task CompleteTaskAsync_TaskNotFound_ThrowsException()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.GetTaskAsync(356))
            .ReturnsAsync((TaskItem?)null);
        
        //Act and Assert
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(
            () => _service.CompleteTaskAsync(356, "john@example.com"));
    }

    [TestMethod]
    public async Task GetPendingTasksForUserAsync_ReturnsOnlyIncompleteTasks()
    {
        //Arrange
        var taskList = new List<TaskItem>()
        {
            new TaskItem{Id = 356, Title ="Finish Report", Description = "Q4 financial report", AssignedTo = "john@example.com", IsCompleted = false},
            new TaskItem{Id = 357, Title ="Finish Report2", Description = "Q3 financial report", AssignedTo = "john@example.com", IsCompleted = true},
            new TaskItem{Id = 358, Title ="Finish Report3", Description = "Q2 financial report", AssignedTo = "john@example.com", IsCompleted = false}
        };

        _mockRepo
            .Setup(r => r.GetTasksByAssigneeAsync("john@example.com"))
            .ReturnsAsync(taskList);
        //Act
        var result = await _service.GetPendingTasksForUserAsync("john@example.com");

        //Assert
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.All(t => !t.IsCompleted));
        Assert.IsFalse(result.Any(t => t.Id == 357));
        Assert.IsTrue(result.Any(t => t.Description.Contains("Q4 financial report")));
        Assert.IsTrue(result.Any(t => t.Description.Contains("Q2 financial report")));
    }

    [TestMethod]
    public async Task DeleteTaskAsync_CallsRepositoryDelete()
    {
        //Arrange
        _mockRepo
            .Setup(r => r.DeleteTaskAsync(It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        //Act
        await _service.DeleteTaskAsync(356);

        //Assert
        _mockRepo.Verify(r => r.DeleteTaskAsync(356), Times.Once);
    }
}