using Microsoft.EntityFrameworkCore;

public class LibraryDbContext : DbContext
{
    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<Member> Members { get; set; }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
}
