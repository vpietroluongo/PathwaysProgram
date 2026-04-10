using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.IO.Pipelines;
using System.Threading.Tasks;

[TestClass]
public class LeadManagerTests
{
    private Mock<IContactAccess> _mockContactAccess;
    private LeadManager _leadManager;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockContactAccess = new Mock<IContactAccess>();
        _leadManager = new LeadManager(_mockContactAccess.Object);
    }

    [TestMethod]
    public async Task PromoteToLeadAsync_ExistingContact_SetsIsLeadToTrue()
    {
        //Arrange
        var contact = new Contact
        {
            Id = 42,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            IsLead = false
        };

        _mockContactAccess
            .Setup(x => x.GetContactAsync(42))
            .ReturnsAsync(contact);
        // _mockContactAccess
        //     .Setup(x => x.SaveContactAsync(contact))
        //     .Returns(Task.CompletedTask);
        _mockContactAccess
            .Setup(x => x.SaveContactAsync(It.IsAny<Contact>()))
            .Returns(Task.CompletedTask);

        //Act
        await _leadManager.PromotedToLeadAsync(42);

        //Assert
        Assert.IsTrue(contact.IsLead);
        _mockContactAccess.Verify(x => x.SaveContactAsync(It.Is<Contact>(c => c.IsLead == true && c.Id == 42)),
        Times.Once);
    }

    [TestMethod]
    public async Task PromoteToLeadAsync_ContactNotFound_ThrowsException()
    {
        //Arrange
        _mockContactAccess
            .Setup(x => x.GetContactAsync(5))
            .ReturnsAsync((Contact?)null);
        
        //Act and Assert
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => _leadManager.PromotedToLeadAsync(5));

        
    }

    [TestMethod]
    public async Task IsLeadAsync_ReturnsCorrectValue()
    {
        //Arrange
        _mockContactAccess
            .Setup(x => x.GetContactAsync(10))
            .ReturnsAsync(new Contact { Id = 10, IsLead = true});
        _mockContactAccess
            .Setup(x => x.GetContactAsync(20))
            .ReturnsAsync(new Contact { Id = 20, IsLead = false});
        
        //Act
        var result10 = await _leadManager.IsLeadAsync(10);
        var result20 = await _leadManager.IsLeadAsync(20);

        //Assert
        Assert.IsTrue(result10);
        Assert.IsFalse(result20);
    }

    [TestMethod]
    public async Task PromoteToLeadAsync_verifySaveIsCalledExactlyOnce()
    {
        //Arrange
        var contact = new Contact { Id = 100, IsLead = false};
        _mockContactAccess
            .Setup(x => x.GetContactAsync(100))
            .ReturnsAsync(contact);
        // _mockContactAccess
        //     .Setup(x => x.SaveContactAsync(contact))
        //     .Returns(Task.CompletedTask);
        _mockContactAccess
            .Setup(x => x.SaveContactAsync(It.IsAny<Contact>()))
            .Returns(Task.CompletedTask);
        
        //Act
        await _leadManager.PromotedToLeadAsync(contact.Id);

        //Assert
        _mockContactAccess.Verify(x => x.SaveContactAsync(It.IsAny<Contact>()), Times.Once);
    }
}
