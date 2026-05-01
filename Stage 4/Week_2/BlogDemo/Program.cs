using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new BlogContext();

        await SeedDataAsync(context);

        Console.WriteLine("=== Simple Blog System - EF + LINQ ===\n");

        // LINQ Query 1: All posts with author name
        var posts = await context.BlogPosts
            .Include(p => p.Author)
            .OrderByDescending(p => p.PublishedDate)
            .Select(p => new
            {
                p.Title,
                Author = p.Author.Name,
                p.PublishedDate
            })
            .ToListAsync();

        Console.WriteLine("📝 Latest Blog Posts:");
        foreach (var post in posts)
        {
            Console.WriteLine($"• {post.Title} by {post.Author} ({post.PublishedDate:yyyy-MM-dd})");
        }

        //LINQ Query 2: Authors with post count
        var authorStats = await context.Authors 
            .Select(a => new
            {
                a.Name,
                PostCount = context.BlogPosts.Count(p => p.AuthorId == a.Id),
                LatestPost = context.BlogPosts
                    .Where(p => p.AuthorId == a.Id)
                    .OrderByDescending(p => p.PublishedDate)
                    .Select(p => p.Title)
                    .FirstOrDefault()
            })
            .ToListAsync();
        
        Console.WriteLine("\n👤 Author Statistics:");
        foreach (var a in authorStats)
        {
            Console.WriteLine($"• {a.Name} - {a.PostCount} posts | Latest: {a.LatestPost}");
        }

        Console.WriteLine("\nDone! Press any key to exit.");
        Console.ReadKey();
    }

    static async Task SeedDataAsync(BlogContext context)
    {
        if (await context.BlogPosts.AnyAsync()) return;

        var authors = new List<Author>
        {
            new Author { Name = "Alice Johnson", Email = "alice@email.com", JoinedDate = DateTime.UtcNow.AddYears(-2) },
            new Author { Name = "Bob Chen", Email = "bob@email.com", JoinedDate = DateTime.UtcNow.AddMonths(-8) } 
        };

        context.Authors.AddRange(authors);
        await context.SaveChangesAsync();

        var posts = new List<BlogPost>
        {
            new BlogPost
            {
                Title = "Getting Started with EF Core",
                Content = "EF Core is powerful...",
                PublishedDate = DateTime.UtcNow.AddDays(-5),
                AuthorId = 1
            },
            new BlogPost
            {
                Title = "LINQ Best Practices",
                Content = "LINQ makes queries easy...",
                PublishedDate = DateTime.UtcNow.AddDays(-2),
                AuthorId = 2
            },
            new BlogPost 
            { 
                Title = "Model Builder Tips", 
                Content = "Configure your entities properly...", 
                PublishedDate = DateTime.UtcNow.AddDays(-10),
                AuthorId = 1 
            }   
        };

        context.BlogPosts.AddRange(posts);
        await context.SaveChangesAsync();
    }
}
