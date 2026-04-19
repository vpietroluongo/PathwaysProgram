using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestClass]
public class LibraryServiceTests
{
    private Mock<LibraryDbContext> _mockContext;  
    private Mock<DbSet<Book>> _mockBooks;         
    private Mock<DbSet<Member>> _mockMembers;     

    private LibraryService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockBooks = new Mock<DbSet<Book>>();    
        _mockMembers = new Mock<DbSet<Member>>();    

        _mockContext = new Mock<LibraryDbContext>(new DbContextOptions<LibraryDbContext>()); 
        _mockContext.Setup(c => c.Books).Returns(_mockBooks.Object); 
        _mockContext.Setup(c => c.Members).Returns(_mockMembers.Object); 

        _service = new LibraryService(_mockContext.Object); 
    }

    // === Your tests go here ===

    [TestMethod]
    public async Task GetAvailableBooksAsync_ReturnsOnlyAvailableBooks_SortedByTitle()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "C# in Depth", IsAvailable = true },
            new Book { Id = 2, Title = "Clean Code", IsAvailable = false },
            new Book { Id = 3, Title = "The Pragmatic Programmer", IsAvailable = true }
        }.AsQueryable();

        // _mockBooks.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.Provider);
        // _mockBooks.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.Expression);
        // _mockBooks.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.ElementType);
        // _mockBooks.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

        _mockContext.Setup(c => c.Books).ReturnsDbSet(books);
        // Act
        var result = await _service.GetAvailableBooksAsync();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("C# in Depth", result[0].Title);
        Assert.AreEqual("The Pragmatic Programmer", result[1].Title);
    }

    [TestMethod]
    public async Task SearchBooksAsync_FindsBooksByTitleOrAuthor()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "C# in Depth", IsAvailable = true },
            new Book { Id = 2, Title = "Clean Code", IsAvailable = false },
            new Book { Id = 3, Title = "The Pragmatic Programmer", IsAvailable = true }
        }.AsQueryable();

        _mockContext.Setup(c => c.Books).ReturnsDbSet(books);

        //Act
        var result = await _service.SearchBooksAsync("clean");

        //Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Clean Code", result[0].Title);
    }

    [TestMethod]
    public async Task BorrowBookAsync_SuccessfulBorrow_ReturnsTrue()
    {
        // Arrange
        var book = new Book
        {
            Id = 256, 
            Title = "C# in Depth", 
            IsAvailable = true 
        };

        var member = new Member
        {
            Id = 101, Name = "Alice", Email = "alice@test.com"
        };

        _mockBooks.Setup(b => b.FindAsync(256)).ReturnsAsync(book);
        _mockMembers.Setup(m => m.FindAsync(101)).ReturnsAsync(member);

        //Act
        var result = await _service.BorrowBookAsync(256, 101);

        //Assert
        Assert.IsTrue(result);
        Assert.IsFalse(book.IsAvailable);
        Assert.IsNotNull(book.BorrowedDate);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task GetOverdueBooksCountAsync_ReturnsCorrectCount()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "C# in Depth", BorrowedDate = DateTime.UtcNow.AddDays(-31), IsAvailable = false },
            new Book { Id = 2, Title = "Clean Code", BorrowedDate = null, IsAvailable = true },
            new Book { Id = 3, Title = "The Pragmatic Programmer", BorrowedDate= DateTime.UtcNow.AddDays(-40), IsAvailable = false },
            new Book { Id = 4, Title = "Book about Books", BorrowedDate= DateTime.UtcNow.AddDays(-29), IsAvailable = false }
        }.AsQueryable();

        _mockContext.Setup(c => c.Books).ReturnsDbSet(books);

        //Act
        var result = await _service.GetOverdueBooksCountAsync();

        //Assert
        Assert.AreEqual(2, result);
    }

    [TestMethod]
    public async Task GetMostBorrowedAuthorAsync_ReturnsCorrectAuthor()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "C# in Depth", Author = "Bob Bob", BorrowedDate = DateTime.UtcNow.AddDays(-31), IsAvailable = false },
            new Book { Id = 2, Title = "Clean Code", Author = "Alice Alice", BorrowedDate = null, IsAvailable = true },
            new Book { Id = 3, Title = "The Pragmatic Programmer", Author = "Charlie Charlie", BorrowedDate= DateTime.UtcNow.AddDays(-40), IsAvailable = false },
            new Book { Id = 4, Title = "Book about Books", Author = "Bob Bob", BorrowedDate= DateTime.UtcNow.AddDays(-29), IsAvailable = false },
            new Book { Id = 5, Title = "Even Cleaner Code", Author = "Alice Alice", BorrowedDate = null, IsAvailable = false }
        }.AsQueryable();

        _mockContext.Setup(c => c.Books).ReturnsDbSet(books);

        //Act
        var result = await _service.GetMostBorrowedAuthorAsync();

        //Assert
        Assert.AreEqual("Bob Bob", result);
    }

    // // Helper method to setup IQueryable mocks
    // private static void SetupQueryableMock<T>(Mock<DbSet<T>> mockSet, IQueryable<T> data) where T : class
    // {

    // }

    // [TestMethod]
    // public async Task SearchBooksAsync_FindsBooksByTitleOrAuthor()
    // {
    //     var books = new List<Book>
    //     {
    //         new Book { Id = 1, Title = "C# Programming", Author = "Jon Skeet" },
    //         new Book { Id = 2, Title = "Clean Architecture", Author = "Robert Martin" },
    //         new Book { Id = 3, Title = "Python Crash Course", Author = "Eric Matthes" }
    //     }.AsQueryable();

    //     //SetupQueryableMock(_mockBooks, books);
    //     _mockContext.Setup(c => c.Books).ReturnsDbSet(books);

    //     var result = await _service.SearchBooksAsync("c#");

    //     Assert.AreEqual(1, result.Count);
    //     Assert.AreEqual("C# Programming", result[0].Title);
    // }

    // [TestMethod]
    // public async Task BorrowBookAsync_SuccessfulBorrow_ReturnsTrue()
    // {
    //     var book = new Book { Id = 10, Title = "Test Book", IsAvailable = true };
    //     var member = new Member { Id = 5, Name = "John Doe" };

    //     _mockBooks.Setup(b => b.FindAsync(10)).ReturnsAsync(book);
    //     _mockMembers.Setup(m => m.FindAsync(5)).ReturnsAsync(member);

    //     var result = await _service.BorrowBookAsync(10, 5);

    //     Assert.IsTrue(result);
    //     Assert.IsFalse(book.IsAvailable);
    //     Assert.IsNotNull(book.BorrowedDate);
    //     _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    // }

    // [TestMethod]
    // public async Task GetOverdueBooksCountAsync_ReturnsCorrectCount()
    // {
    //     var books = new List<Book>
    //     {
    //         new Book { Id = 1, IsAvailable = false, BorrowedDate = DateTime.UtcNow.AddDays(-45) },
    //         new Book { Id = 2, IsAvailable = true, BorrowedDate = null },
    //         new Book { Id = 3, IsAvailable = false, BorrowedDate = DateTime.UtcNow.AddDays(-15) },
    //         new Book { Id = 4, IsAvailable = false, BorrowedDate = DateTime.UtcNow.AddDays(-35) }
    //     }.AsQueryable();

    //     //SetupQueryableMock(_mockBooks, books);
    //     _mockContext.Setup(c => c.Books).ReturnsDbSet(books);

    //     int overdue = await _service.GetOverdueBooksCountAsync();

    //     Assert.AreEqual(2, overdue);
    // }

    // Helper method to setup IQueryable mocks
    // private static void SetupQueryableMock<T>(Mock<DbSet<T>> mockSet, IQueryable<T> data) where T : class
    // {
    //     mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
    //     mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
    //     mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
    //     mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
    // }
}
