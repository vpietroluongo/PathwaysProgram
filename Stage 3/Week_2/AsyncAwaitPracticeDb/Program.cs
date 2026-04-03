using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

var context = new BookContext();

// TODO: Complete the 4 activities below

//Run synchronous version
AddBooksSync();
LoadBooksSync();

//Run asynchronous version
await AddBooksAsync();
await LoadBooksAsync();


// ====================== SYNCHRONOUS METHODS ======================
void AddBooksSync()
{
    var books = new List<Book>
    {
        new Book
        { 
            Title = "Pride and Prejudice",
            Author = "Jane Austen",
            Year = 1813
        },
        new Book
        {
            Title = "1984",
            Author = "George Orwell",
            Year = 1949
        },
        new Book
        {
            Title = "Project Hail Mary",
            Author = "Andy Weir",
            Year = 2021
        }
    };

    context.Books.AddRange(books);
    context.SaveChanges();
    Console.WriteLine("Books added synchronously.");
}

void LoadBooksSync()
{
    var books = context.Books.ToList();
    Console.WriteLine("Books loaded synchronously.");

    foreach(var book in books)
    {
        Console.WriteLine($"{book.Title} by {book.Author} ({book.Year})");
    }
}

// ====================== ASYNCHRONOUS METHODS ======================
async Task AddBooksAsync()
{
    var books = new List<Book>
    {
        new Book {Title = "Remarkably Bright Creatures", Author = "Shelby Van Pelt", Year = 2022},
        new Book {Title = "Stowaway", Author = "Joe Shute", Year = 2025},
        new Book {Title = "The Huckleberry Book", Author = "Asta Bowen", Year = 1988}
    };

    await context.Books.AddRangeAsync(books);
    await context.SaveChangesAsync();
    Console.WriteLine("Books added asynchronously.");
}

async Task LoadBooksAsync()
{
    var books = await context.Books.ToListAsync();
    Console.WriteLine("Books loaded asynchronously");

    foreach (var book in books)
    {
        Console.WriteLine($"{book.Title} by {book.Author} ({book.Year})");
    }

}


class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
}

class BookContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("BooksDb");
    }
}
