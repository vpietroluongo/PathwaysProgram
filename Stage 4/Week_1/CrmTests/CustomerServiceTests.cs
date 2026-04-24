using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestClass]
public class CustomerServiceTests
{
    private CrmDbContext _context;
    private CustomerService _service;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<CrmDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CrmDbContext(options);
        _service = new CustomerService(_context);

        // Seed some test data
        SeedTestData();
    }

    private void SeedTestData()
    {
        _context.Customers.AddRange(new List<Customer>
        {
            new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@email.com", Status = "Lead", CreatedDate = DateTime.UtcNow },
            new Customer { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@email.com", Status = "Customer", CreatedDate = DateTime.UtcNow },
            new Customer { Id = 3, FirstName = "Bob", LastName = "Johnson", Email = "bob@email.com", Status = "Prospect", CreatedDate = DateTime.UtcNow.AddDays(-5) }
        });
        _context.SaveChanges();
    }

    // === Write your tests here ===

    [TestMethod]
    public async Task AddCustomerAsync_ValidCustomer_AddsSuccessfully()
    {
        // Test arrays, if/else, etc.
        //Arrange
        var customer = new Customer
        {
           FirstName = "Charlie",
           LastName = "Jones",
           Email = "charlie@example.com", 
        };

        //Act 
        var result = await _service.AddCustomerAsync(customer);

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Lead", result.Status);
        Assert.AreEqual(4, result.Id);
        Assert.IsTrue(result.Id > 0);
        Assert.AreNotEqual(default(DateTime), result.CreatedDate);
    }

    [TestMethod]
    public async Task GetCustomersByStatusAsync_ReturnsCorrectCustomers()
    {
        //Arrange
        var status = "Lead";

        //Act
        var result = await _service.GetCustomersByStatusAsync(status);

        //Assert
        Assert.IsTrue(result.Any(r => r.Id == 1));
        Assert.HasCount(1, result);
        Assert.IsTrue(result.All(r => r.Status == "Lead"));
    }

    [TestMethod]
    public async Task UpdateCustomerStatusAsync_ValidStatus_UpdatesSuccessfully()
    {
        //Arrange
        var customerId = 3;
        var status = "Customer";

        //Act
        await _service.UpdateCustomerStatusAsync(customerId, status);

        //Assert
        var updated = await _context.Customers.FindAsync(customerId);
        Assert.AreEqual(status, updated.Status);
    }

    [TestMethod]
    public async Task CountCustomersByStatusAsync_ReturnsCorrectCounts()
    {
        //Act
        var result = await _service.CountCustomersByStatusAsync();

        //Assert
        Assert.IsTrue(result.ContainsKey("Lead"));
        Assert.IsTrue(result.ContainsKey("Customer"));
        Assert.IsTrue(result.ContainsKey("Prospect"));
        Assert.AreEqual(1, result["Lead"]);
    }
}