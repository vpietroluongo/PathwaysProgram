using Microsoft.EntityFrameworkCore;
using System;

var context = new LibraryContext();
SeedData(context);

Console.WriteLine("Library Database is ready!\n");

//Activity 1: List all books
Console.WriteLine("\n=== Activity 1 ===");
var allBooks = context.Books
    .ToList();
foreach(var book in allBooks)
{
    Console.WriteLine($"{book.Title} by {book.Author}");
}


//Activity 2: Show books by Fantasy genre
Console.WriteLine("\n=== Activity 2 ===");
var fantasy = context.Books
    .Where(b => b.Genre == "Fantasy")
    .ToList();
foreach(var book in fantasy)
{
    Console.WriteLine($"- {book.Title}");
}


//Activity 3: Add a new book
Console.WriteLine("\n=== Activity 3 ===");
var newBook = new Book
{
    Title = "1984",
    Author = "George Orwell",
    Year = 1949,
    Genre = "Fiction"
};
context.Books.Add(newBook);
context.SaveChanges();

var newBookCheck = context.Books
    .FirstOrDefault(b => b.Title == "1984");
if (newBookCheck is not null)
{
    Console.WriteLine("Book successfully added.");
}


//Activity 4: Update book year
Console.WriteLine("\n=== Activity 4 ===");
var dune = context.Books
    .FirstOrDefault(b => b.Title == "Dune");
if (dune is not null)
{
    dune.Year = 1966;
    context.SaveChanges();

    var duneCheck = context.Books.FirstOrDefault(b => b.Title == "Dune");
    if (dune.Year == 1966)
    {
        Console.WriteLine("Book successfully updated.");
    }
}


//Activity 5: Show books published after 1950
Console.WriteLine("\n=== Activity 5 ===");
var after1950 = context.Books
    .Where(b => b.Year > 1950)
    .ToList();
foreach (var book in after1950)
{
    Console.WriteLine($"{book.Title} {book.Year}");
}

// ====================== SEED DATA ======================
void SeedData(LibraryContext ctx)
{
    if (ctx.Books.Any()) return;

    var books = new List<Book>
    {
        new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Year = 1937, Genre = "Fantasy" },
        new Book { Title = "Dune", Author = "Frank Herbert", Year = 1965, Genre = "Sci-Fi" },
        new Book { Title = "Pride and Prejudice", Author = "Jane Austen", Year = 1813, Genre = "Romance" },
        new Book { Title = "The Alchemist", Author = "Paulo Coelho", Year = 1988, Genre = "Fiction" },
        new Book { Title = "Harry Potter and the Sorcerer's Stone", Author = "J.K. Rowling", Year = 1997, Genre = "Fantasy" }
    };

    ctx.Books.AddRange(books);
    ctx.SaveChanges();
    Console.WriteLine("5 books seeded successfully.\n");
}

// ====================== MODELS ======================
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
}

public class LibraryContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("LibraryDb");
    }
}
