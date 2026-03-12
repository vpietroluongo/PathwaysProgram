using Microsoft.EntityFrameworkCore;

namespace MyBackpack.Models;

public class MyBackpackContext : DbContext
{
    public MyBackpackContext(DbContextOptions<MyBackpackContext> options) : base(options)
    { }

    public DbSet<MyBackpackItem> MyBackpackItems { get; set; } = null;
}

