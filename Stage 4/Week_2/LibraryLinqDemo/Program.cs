using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new LibraryContext();
        await SeedDataAsync(context);

        Console.WriteLine("=== Library System - LINQ Practice ===\n");

        // 1. Basic Filtering
        Console.WriteLine("1. Books published after 2015:");
        var recentBooks = await context.Books
            .Where(b => b.YearPublished > 2015)
            .OrderByDescending(b => b.YearPublished)
            .ToListAsync();

        recentBooks.ForEach(b => Console.WriteLine($"   {b.Title} ({b.YearPublished}) - ${b.Price}"));

        // 2. Projection (Select)
        Console.WriteLine("\n2. Book Titles and Authors (Projection):");
        var bookInfo = await context.Books
            .Select(b => new { b.Title, b.Author, b.Genre })
            .Take(5)
            .ToListAsync();

        bookInfo.ForEach(b => Console.WriteLine($"   {b.Title} by {b.Author} [{b.Genre}]"));

        // 3. Grouping + Aggregation
        Console.WriteLine("\n3. Books by Genre:");
        var booksByGenre = await context.Books
            .GroupBy(b => b.Genre)
            .Select(g => new 
            { 
                Genre = g.Key, 
                Count = g.Count(),
                AvgPrice = g.Average(b => b.Price)
            })
            .OrderByDescending(g => g.Count)
            .ToListAsync();

        foreach (var g in booksByGenre)
        {
            Console.WriteLine($"   {g.Genre}: {g.Count} books | Avg Price: ${g.AvgPrice:F2}");
        }

        // 4. Joining + Navigation Properties
        Console.WriteLine("\n4. Active Loans with Member and Book Info:");
        var activeLoans = await context.Loans
            .Where(l => l.ReturnDate == null)
            .Include(l => l.Member)
            .Include(l => l.Book)
            .Select(l => new 
            { 
                Member = l.Member.FirstName + " " + l.Member.LastName,
                Book = l.Book.Title,
                LoanDate = l.LoanDate
            })
            .ToListAsync();

        activeLoans.ForEach(l => Console.WriteLine($"   {l.Member} borrowed '{l.Book}'"));

        // 5. Complex Query - Members who borrowed expensive books
        Console.WriteLine("\n5. Members who borrowed books over $30:");
        var bigSpenders = await context.Members
            .Where(m => m.Loans.Any(l => l.Book.Price > 30))
            .Select(m => new 
            { 
                m.FirstName, 
                m.LastName, 
                BorrowedBooks = m.Loans.Count 
            })
            .ToListAsync();

        bigSpenders.ForEach(m => Console.WriteLine($"   {m.FirstName} {m.LastName} - {m.BorrowedBooks} books"));

        // 6. Find most borrowed book
        Console.WriteLine("\n6. Most borrowed book:");
        var mostBorrowed = await context.Loans
            .Include(l => l.Book) 
            .GroupBy(l => new {l.BookId, l.Book.Title, l.Book.Author} )
            .Select(g => new
            {
                Title = g.Key.Title,
                Author = g.Key.Author,
                BorrowedCount = g.Count()    
            })
            .OrderByDescending(g => g.BorrowedCount)
            .FirstOrDefaultAsync();
        
        Console.WriteLine($"   {mostBorrowed.Title} borrowed {mostBorrowed.BorrowedCount} times");

        // 7. Query using Join() instead of navigaton properties
        Console.WriteLine("\n7. Join() instead of navigation properties:");
        var activeLoans2 = await context.Loans
            .Where(l => l.ReturnDate == null)
            .Join(context.Members,
                loan => loan.MemberId,
                member => member.Id,
                (loan, member) => new { loan, member })
            .Join(context.Books,
                  x => x.loan.BookId,
                  book => book.Id,
                  (x, book) => new
                  {
                      Member = x.member.FirstName + " " + x.member.LastName,
                      Book = book.Title,
                      LoanDate = x.loan.LoanDate
                  })
            .ToListAsync();

        activeLoans2.ForEach(l => Console.WriteLine($"   {l.Member} borrowed '{l.Book}'"));
        
        // 8. Overdue Books
        Console.WriteLine("\n8. Overdue books:");
        var overdueBooks = await context.Loans
            .Where(l => l.ReturnDate == null && l.LoanDate < DateTime.UtcNow.AddDays(-30))
            .Include(l => l.Book)
            .Select(g => new
            {
                Title = g.Book.Title,
                LoanDate = g.LoanDate
            })
            .ToListAsync();
        
        overdueBooks.ForEach(b => Console.WriteLine($"   {b.Title} loaned {b.LoanDate}"));

        Console.WriteLine("\nLINQ Practice Completed!");
        Console.ReadKey();
    }

    static async Task SeedDataAsync(LibraryContext context)
    {
        if (await context.Books.AnyAsync()) return;

        var books = new List<Book>
        {
            new Book { Title = "The Clean Code", Author = "Robert Martin", Genre = "Programming", YearPublished = 2008, Price = 45.99m },
            new Book { Title = "Dune", Author = "Frank Herbert", Genre = "Sci-Fi", YearPublished = 1965, Price = 25.99m },
            new Book { Title = "Project Hail Mary", Author = "Andy Weir", Genre = "Sci-Fi", YearPublished = 2021, Price = 32.50m },
            new Book { Title = "Atomic Habits", Author = "James Clear", Genre = "Self-Help", YearPublished = 2018, Price = 18.99m }
        };

        var members = new List<Member>
        {
            new Member { FirstName = "Sarah", LastName = "Chen", Email = "sarah@email.com", JoinDate = DateTime.UtcNow.AddMonths(-6) },
            new Member { FirstName = "Miguel", LastName = "Rodriguez", Email = "miguel@email.com", JoinDate = DateTime.UtcNow.AddMonths(-3) }
        };

        context.Books.AddRange(books);
        context.Members.AddRange(members);
        await context.SaveChangesAsync();

        // Create some loans
        var loans = new List<Loan>
        {
            new Loan { BookId = 1, MemberId = 1, LoanDate = DateTime.UtcNow.AddDays(-10) },
            new Loan { BookId = 3, MemberId = 2, LoanDate = DateTime.UtcNow.AddDays(-5) },
            new Loan { BookId = 4, MemberId = 1, LoanDate = DateTime.UtcNow.AddDays(-31) },
            new Loan { BookId = 3, MemberId = 1, LoanDate = DateTime.UtcNow.AddDays(-50), ReturnDate = DateTime.UtcNow.AddDays(-15) },
        };

        context.Loans.AddRange(loans);
        await context.SaveChangesAsync();
    }
}