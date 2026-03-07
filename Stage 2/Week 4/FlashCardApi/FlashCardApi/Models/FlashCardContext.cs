using Microsoft.EntityFrameworkCore;

namespace FlashCardApi.Models;

public class FlashCardContext : DbContext
{
    public FlashCardContext(DbContextOptions<FlashCardContext> options)
        : base(options)
    {
    }

    public DbSet<FlashCardItem> FlashCardItems { get; set; } = null!;
}