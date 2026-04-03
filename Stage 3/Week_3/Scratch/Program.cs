using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// === Models & DbContext here ===
class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}

class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Product> Products { get; set; } = new();
}

class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ModernDemoDb");
    }
}

await using var db = new AppDbContext();

await SeedData(db);
await GetAllProducts(db);
await GetExpensiveProducts(db);
await GetProductsWithCategory(db);
await AddNewProduct(db);
await UpdateProductPrice(db);
await DeleteProduct(db);
await CountProductsPerCategory(db);
await GetMostExpensiveProduct(db);

Console.WriteLine("\nAll 10 activities completed successfully!");

Console.ReadLine();

async Task SeedData(AppDbContext db)
{
    if (await db.Categories.AnyAsync()) return;

    var electronics = new Category { Name = "Electronics" };
    var books = new Category { Name = "Books" };

    db.Categories.AddRange(electronics, books);
    await db.SaveChangesAsync();

    db.Products.AddRange(
        new Product { Name = "Laptop", Price = 999.99m, CategoryId = electronics.Id },
        new Product { Name = "Phone", Price = 599.99m, CategoryId = electronics.Id },
        new Product { Name = "C# Programming", Price = 39.99m, CategoryId = books.Id },
        new Product { Name = "Clean Code", Price = 45.50m, CategoryId = books.Id }
    );

    await db.SaveChangesAsync();
    Console.WriteLine("Data seeded successfully.");
}

async Task GetAllProducts(AppDbContext db)
{
    var products = await db.Products.ToListAsync();
    foreach (var p in products)
    {
        Console.WriteLine(p.Name);
    }
}

async Task GetExpensiveProducts(AppDbContext db)
{
    var expensive = await db.Products
        .Where(p => p.Price > 100)
        .ToListAsync();

    foreach (var p in expensive)
    {
        Console.WriteLine($"{p.Name} - ${p.Price}");
    }
}

async Task GetProductsWithCategory(AppDbContext db)
{
    var products = await db.Products
        .Include(p => p.Category)
        .ToListAsync();

    foreach (var p in products)
    {
        Console.WriteLine($"{p.Name} belongs to {p.Category?.Name}");
    }
}

async Task AddNewProduct(AppDbContext db)
{
    var electronics = await db.Categories.FirstAsync(c => c.Name == "Electronics");

    var tablet = new Product 
    { 
        Name = "Tablet", 
        Price = 399.99m, 
        CategoryId = electronics.Id 
    };

    db.Products.Add(tablet);
    await db.SaveChangesAsync();
    Console.WriteLine("Tablet added successfully.");
}

async Task UpdateProductPrice(AppDbContext db)
{
    var phone = await db.Products.FirstOrDefaultAsync(p => p.Name == "Phone");
    if (phone != null)
    {
        phone.Price += 50;
        await db.SaveChangesAsync();
        Console.WriteLine($"Updated price for Phone: ${phone.Price}");
    }
}

async Task DeleteProduct(AppDbContext db)
{
    var book = await db.Products.FirstOrDefaultAsync(p => p.Name == "Clean Code");
    if (book != null)
    {
        db.Products.Remove(book);
        await db.SaveChangesAsync();
        Console.WriteLine("Clean Code deleted.");
    }
}

async Task CountProductsPerCategory(AppDbContext db)
{
    var counts = await db.Categories
        .Select(c => new 
        { 
            Category = c.Name, 
            Count = c.Products.Count 
        })
        .ToListAsync();

    foreach (var item in counts)
    {
        Console.WriteLine($"{item.Category}: {item.Count} products");
    }
}

async Task GetMostExpensiveProduct(AppDbContext db)
{
    var expensive = await db.Products
        .OrderByDescending(p => p.Price)
        .FirstOrDefaultAsync();

    if (expensive != null)
    {
        Console.WriteLine($"Most expensive: {expensive.Name} - ${expensive.Price}");
    }
}