using Microsoft.EntityFrameworkCore;

public class LibraryService
{
    private readonly LibraryDbContext _context;

    public LibraryService(LibraryDbContext context)
    {
        _context = context;
    }

    // LINQ-heavy methods
    public async Task<List<Book>> GetAvailableBooksAsync()
    {
        return await _context.Books
            .Where(b => b.IsAvailable)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<List<Book>> SearchBooksAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return new List<Book>();

        keyword = keyword.ToLower();

        return await _context.Books
            .Where(b => b.Title.ToLower().Contains(keyword) ||
                        b.Author.ToLower().Contains(keyword) ||
                        b.ISBN.Contains(keyword))
            .ToListAsync();
    }

    public async Task<bool> BorrowBookAsync(int bookId, int memberId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null || !book.IsAvailable)
            return false;

        var member = await _context.Members.FindAsync(memberId);
        if (member == null)
            return false;

        book.IsAvailable = false;
        book.BorrowedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetOverdueBooksCountAsync()
    {
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

        return await _context.Books
            .CountAsync(b => !b.IsAvailable && b.BorrowedDate.HasValue && b.BorrowedDate.Value < thirtyDaysAgo);
    }

    public async Task<string?> GetMostBorrowedAuthorAsync()
    {
        return await _context.Books
            .Where(b => !b.IsAvailable)
            .GroupBy(b => b.Author)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync();    
    }
}
