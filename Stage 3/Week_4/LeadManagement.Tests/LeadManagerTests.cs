using Moq;

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
    public async Task PromoteToLeadAsync_ExistingContact_SetIsLeadToTrue()
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

        _mockContactAccess
            .Setup(x => x.SaveContactAsync(It.IsAny<Contact>()))
            .Returns(Task.CompletedTask);
        
        //Act
        await _leadManager.PromotedToLeadAsync(42);

        //Assert
        Assert.IsTrue(contact.IsLead);
        _mockContactAccess.Verify(x => x.SaveContactAsync(
            It.Is<Contact>(c => c.IsLead == true && c.Id == 42)),
            Times.Once);
    }

    [TestMethod]
    public async Task PromoteToLeadAsync_ContactNotFound_ThrowsException()
    {
        //Arrange
        _mockContactAccess
            .Setup(x => x.GetContactAsync(999))
            .ReturnsAsync((Contact?)null);
        
        //Act & Assert
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(
            () => _leadManager.PromotedToLeadAsync(999));
    }
    
    [TestMethod]
    public async Task IsLeadAsync_ReturnsCorrectValue()
    {
        //Arrange
        _mockContactAccess  
            .Setup(x => x.GetContactAsync(10))
            .ReturnsAsync(new Contact { Id = 10, IsLead = true });
        
        _mockContactAccess
            .Setup(x => x.GetContactAsync(20))
            .ReturnsAsync(new Contact { Id = 20, IsLead = false});

        //Act
        bool isLead10 = await _leadManager.IsLeadAsync(10);
        bool isLead20 = await _leadManager.IsLeadAsync(20);

        //Assert
        Assert.IsTrue(isLead10);
        Assert.IsFalse(isLead20);
        _mockContactAccess.Verify(x => x.GetContactAsync(It.IsAny<int>()), Times.Exactly(2));
        _mockContactAccess.Verify(x => x.GetContactAsync(It.Is<int>(i => i == 20)), Times.Once);
    }

    [TestMethod]
    public async Task PromoteToLeadAsync_VerifySaveIsCalledExactlyOnce()
    {
        //Arrange
        var contact = new Contact { Id = 100, IsLead = false};
        _mockContactAccess.Setup(x => x.GetContactAsync(100)).ReturnsAsync(contact);
        _mockContactAccess.Setup(x => x.SaveContactAsync(It.IsAny<Contact>())).Returns(Task.CompletedTask);

        //Act
        await _leadManager.PromotedToLeadAsync(100);

        //Assert
        _mockContactAccess.Verify(x => x.SaveContactAsync(It.IsAny<Contact>()), Times.Once);
    }
}
