using Microsoft.EntityFrameworkCore;
using System;

var context = new ShopContext();
SeedData(context);

Console.WriteLine("eCommerce Database is ready!\n");

//Activity 1: List all products
Console.WriteLine("\n== Activity 1 ==");
var allProducts = context.Products
    .ToList();
foreach (var product in allProducts)
{
    Console.WriteLine($"Name: {product.Name} | Price: ${product.Price} | Stock: {product.Stock}");
}


//Activity 2: Show only Electronics products
Console.WriteLine("\n== Activity 2 ==");
var electronics = context.Products
    .Where(p => p.Category == "Electronics")
    .ToList();
foreach (var product in electronics)
{
    Console.WriteLine($"{product.Name} ${product.Price}");
}


//Activity 3: Add a new product
Console.WriteLine("\n== Activity 3 ==");
var newProduct = new Product
{
    Name = "Blue Pen",
    Price = 2.99m,
    Category = "Stationary",
    Stock = 150
};
context.Products.Add(newProduct);
await context.SaveChangesAsync();

var newProductCheck = context.Products
    .FirstOrDefault(p => p.Name == "Blue Pen");
if (newProductCheck is not null)
{
    Console.WriteLine($"{newProductCheck.Name} successfully added");
}


//Activity 4: Update stock of a product
Console.WriteLine("\n== Activity 4 ==");
var coffeeMug = context.Products
    .FirstOrDefault(p => p.Name == "Coffee Mug");
coffeeMug.Stock = 200;
context.SaveChanges();

var coffeeMugCheck = context.Products
    .FirstOrDefault(p => p.Name == "Coffee Mug");
Console.WriteLine($"{coffeeMugCheck.Name} stock is now {coffeeMugCheck.Stock}");


//Activity 5: Show products that cost less than $20
Console.WriteLine("\n== Activity 5 ==");
var costLessThan20 = context.Products
    .Where(p => p.Price < 20)
    .ToList();
foreach (var product in costLessThan20)
{
    Console.WriteLine($"{product.Name} ${product.Price}");
}

// ====================== SEED DATA ======================
void SeedData(ShopContext ctx)
{
    if (ctx.Products.Any()) return;

    var products = new List<Product>
    {
        new Product { Name = "Wireless Headphones", Price = 79.99m, Category = "Electronics", Stock = 25 },
        new Product { Name = "Coffee Mug", Price = 12.99m, Category = "Home", Stock = 120 },
        new Product { Name = "Notebook", Price = 5.49m, Category = "Stationery", Stock = 80 },
        new Product { Name = "USB Cable", Price = 8.99m, Category = "Electronics", Stock = 60 },
        new Product { Name = "Water Bottle", Price = 15.99m, Category = "Sports", Stock = 45 }
    };

    ctx.Products.AddRange(products);
    ctx.SaveChanges();
    Console.WriteLine("5 products seeded successfully.\n");
}

// ====================== MODELS ======================
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public int Stock { get; set; }
}

public class ShopContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ShopDb");
    }
}
