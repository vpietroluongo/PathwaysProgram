using Microsoft.EntityFrameworkCore;

public class BlogContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("BlogDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // === Model Builder Configuration ===

        modelBuilder.Entity<Author>()
            .Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        modelBuilder.Entity<Author>()
            .Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<BlogPost>()
            .Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        modelBuilder.Entity<BlogPost>()
            .Property(p => p.Content)
            .HasMaxLength(2000);
        
        //Relationship Configuration
        modelBuilder.Entity<BlogPost>()
            .HasOne(p => p.Author)
            .WithMany()
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}