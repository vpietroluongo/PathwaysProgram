using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Modern C# Features Practice Started\n");

// TODO: Complete the 10 activities below
var context = new AppDbContext();

//Activity 1: Seed some initial data
await SeedData(context);

//Activity 2: Get all products asynchronously
Console.WriteLine("\n== Activity 2 ==");
await GetAllProducts(context);


//Activity 3: Filter products by price using LINQ
Console.WriteLine("\n== Activity 3 ==");
await GetExpensiveProducts(context);


//Activity 4: Include related data (Category)
Console.WriteLine("\n== Activity 4 ==");
await GetProductsWithCategory(context);


//Activity 5: Add a new product asynchronously
Console.WriteLine("\n== Activity 5 ==");
await AddNewProduct(context);


//Activity 6: Update a product price
Console.WriteLine("\n== Activity 6 ==");
await UpdateProductPrice(context);


//Activity 7: Delete a product
Console.WriteLine("\n== Activity 7 ==");
await DeleteProduct(context);


//Activity 8: Count products per category using LINQ
Console.WriteLine("\n== Activity 8 ==");
await CountProductsPerCategory(context);


//Activity 9: Find the most expensive product
Console.WriteLine("\n== Activity 9 ==");
await GetMostExpensiveProduct(context);



//Activity 10: Combine everything in Main flow
Console.WriteLine("\n== Activity 10 ==");
Console.WriteLine("All 10 activities are completed");

Console.WriteLine("\nAll activities completed. Press ENTER to exit...");
Console.ReadLine();

async Task SeedData(AppDbContext db)
{
    if (await db.Categories.AnyAsync()) return;    //check if there is anything already in Categories

    var electronics = new Category{Name = "Electronics"};
    var stationary = new Category{Name = "Stationary"};

    db.Categories.AddRange(electronics, stationary);
    await db.SaveChangesAsync();

    db.Products.AddRange(
        new Product{Name = "Phone", Price = 199.99m, CategoryId = electronics.Id},
        new Product{Name = "Headphones", Price = 15.99m, CategoryId = electronics.Id},
        new Product{Name = "Pen", Price = 2.75m, CategoryId = stationary.Id},
        new Product{Name = "Notebook", Price = 7.15m, CategoryId = stationary.Id}
    );
    await db.SaveChangesAsync();
    Console.WriteLine("Database seeded successfully");
}

async Task GetAllProducts(AppDbContext db)
{
    var allProducts = await db.Products
        .Select(p => p.Name)
        .ToListAsync();

    foreach(var product in allProducts)
    {
        Console.WriteLine(product);
    } 
}

async Task GetExpensiveProducts(AppDbContext db)
{
    var costMoreThan100 = await db.Products
        .Where(p => p.Price > 100)
        .ToListAsync();

    foreach (var product in costMoreThan100)
    {
    Console.WriteLine($"{product.Name} - ${product.Price}");
    }
}

async Task GetProductsWithCategory(AppDbContext db)
{
    var productAndCategoryInfo = await db.Products 
        .Include(p => p.Category)
        .ToListAsync();

    foreach (var product in productAndCategoryInfo)
    {
        Console.WriteLine($"{product.Name} - {product.Category?.Name}");
    }
}

async Task AddNewProduct(AppDbContext db)
{
    var electronics = await db.Categories
        .FirstAsync(c => c.Name == "Electronics");

    var newProduct = new Product
    {
        Name = "Tablet",
        Price = 399.99m,
        CategoryId = electronics.Id
    };

    db.Products.Add(newProduct);
    await db.SaveChangesAsync();

    var newProductCheck = await db.Products
        .FirstOrDefaultAsync(p => p.Name == newProduct.Name);
    if (newProductCheck is not null)
    {
        Console.WriteLine("Product successfully added.");
    }
}

async Task UpdateProductPrice(AppDbContext db)
{
    var phone = await db.Products
        .FirstOrDefaultAsync(p => p.Name == "Phone");
    if (phone is not null)
    {
        phone.Price += 50;
        await db.SaveChangesAsync();
        Console.WriteLine($"Updated price of phone = {phone.Price}");
    }
}

async Task DeleteProduct(AppDbContext db)
{
    var pen = await db.Products
        .FirstOrDefaultAsync(p => p.Name == "Pen");
    if (pen is not null)
    {
        db.Products.Remove(pen);
        await db.SaveChangesAsync();

        pen = await db.Products.FirstOrDefaultAsync(p => p.Name == "Pen");
        if (pen is null)
        {
            Console.WriteLine("Product successfully deleted.");
        }
    }
}

async Task CountProductsPerCategory(AppDbContext db)
{
    var counts = await db.Categories
        .Select(c => new
        {
            Category = c.Name,
            c.Products.Count
        })
        .ToListAsync();

    foreach (var item in counts)
    {
        var verb = item.Count == 1 ? "is" : "are";
        Console.WriteLine($"There {verb} {item.Count} products in {item.Category}");
    }
}

async Task GetMostExpensiveProduct(AppDbContext db)
{
    var mostExpensive = await db.Products
        .OrderByDescending(p => p.Price)
        .FirstAsync();

    Console.WriteLine($"The most expensive product is {mostExpensive.Name} at ${mostExpensive.Price}.");
}

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