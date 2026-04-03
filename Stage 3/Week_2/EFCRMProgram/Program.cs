using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;

var context = new CrmContext();
SeedData(context);

Console.WriteLine("EF CRM Database is ready!\n");

//Actity 1: List all customers with their order count
Console.WriteLine("\n== Activity 1 ==");
var allCustomers = context.Customers
    .Include(c => c.Orders)
    .ToList();
foreach (var customer in allCustomers)
{
    Console.WriteLine($"{customer.FirstName} {customer.LastName} has {customer.Orders.Count} order(s).");
}


//Activity 2: Show full details of Liam Chen
Console.WriteLine("\n== Activity 2 ==");
var liamDetails = context.Customers
    .Include(c => c.Orders)
    .FirstOrDefault(c => c.FirstName == "Liam" && c.LastName == "Chen");
if (liamDetails != null)
{
    Console.WriteLine($"Name: {liamDetails.FirstName} {liamDetails.LastName}");
    Console.WriteLine($"Date of birth: {liamDetails.DateOfBirth}");
    Console.WriteLine($"Phone: {liamDetails.Phone}, Email: {liamDetails.Email}");
    Console.WriteLine($"Order Count: {liamDetails.Orders.Count}");
    foreach (var order in liamDetails.Orders)
    {
        Console.WriteLine($"   Order Date: {order.OrderDate}, Amount: {order.TotalAmount}, Status: {order.Status}");
    }
}


//Activity 3:  Add a new customer
Console.WriteLine("\n== Activity 3 ==");
var newCustomer = new Customer
{
    FirstName = "Bob",
    LastName = "Smith",
    Email = "bob@email.com",
    Phone = "555-5555",
    DateOfBirth = new DateTime(1990, 1, 1)
};

context.Customers.Add(newCustomer);
await context.SaveChangesAsync();

var newCustomerCheck = context.Customers
    .Include(c => c.Orders)
    .FirstOrDefault(c => c.FirstName == "Bob" && c.LastName == "Smith");
if (newCustomerCheck != null)
{
    Console.WriteLine($"{newCustomerCheck.FirstName} {newCustomerCheck.DateOfBirth}");
}


//Activity 4: Add an order for Emma Johnson
Console.WriteLine("\n== Activity 4 ==");
var emma = context.Customers.FirstOrDefault(c => c.FirstName == "Emma" && c.LastName == "Johnson");
var newOrder = new Order
{
    CustomerId = emma.Id,
    OrderDate = new DateTime(2026, 3, 25, 9, 38, 24),
    TotalAmount = 105.79m,
    Status = "Pending"
};
context.Orders.Add(newOrder);
context.SaveChanges();

var newOrderCheck = context.Customers
    .Include(c => c.Orders)
    .FirstOrDefault(c => c.FirstName == "Emma" && c.LastName == "Johnson");
if (newOrderCheck != null)
{
    Console.WriteLine($"{newOrderCheck.FirstName} {newOrderCheck.LastName} now has {newOrderCheck.Orders.Count} order(s)");
    foreach (var order in newOrderCheck.Orders)
    {
        Console.WriteLine($"   Order Date: {order.OrderDate}, Amount: {order.TotalAmount}, Status: {order.Status}");
    }
}

//Activity 11: Delete order
// Console.WriteLine("\n== Remove Order ==");
// var removeOrder = context.Orders
//     .FirstOrDefault(o => o.TotalAmount == 249.99m);
// context.Orders.Remove(removeOrder);
// context.SaveChanges();

//Activity 5: List All Completed Orders
Console.WriteLine("\n== Activity 5 ==");
var completedOrders = context.Orders
    .Include(o => o.Customer)
    .Where(o => o.Status == "Completed")
    .ToList();
foreach (var order in completedOrders)
{
    Console.WriteLine($"{order.Customer.Email} Order Date: {order.OrderDate}, Amount: {order.TotalAmount}, Status: {order.Status}");
}


//Activity 6: Update Sophia's Phone Number
Console.WriteLine("\n== Activity 6 ==");
var sophia = context.Customers
    .FirstOrDefault(c => c.FirstName == "Sophia" && c.LastName == "Rodriguez");
sophia.Phone = "555-2109";
context.SaveChanges();

var sophiaCheck = context.Customers 
    .FirstOrDefault(c => c.FirstName == "Sophia" && c.LastName == "Rodriguez");
Console.WriteLine(sophiaCheck.Phone);



//Activity 7: Show total revenue from all orders
Console.WriteLine("\n== Activity 7 ==");
var totalRevenue = context.Orders
    .Sum(o => o.TotalAmount);
Console.WriteLine($"Total revenue is ${totalRevenue}");


//Activity 8: Find customers born after 1995
Console.WriteLine("\n== Activity 8 ==");
var bornAfter1995 = context.Customers
    .OrderBy(c => c.DateOfBirth)
    .Where(c => c.DateOfBirth.Year > 1995)
    .ToList();
foreach (var person in bornAfter1995)
{
    Console.WriteLine($"{person.FirstName} {person.LastName} born in {person.DateOfBirth.Year}");
}


//Activity 9: Remove Liam Chen
Console.WriteLine("\n== Activity 9 ==");
var liam = context.Customers
   // .Include(c => c.Orders)
    .FirstOrDefault(c => c.FirstName == "Liam" && c.LastName == "Chen");
context.Customers.Remove(liam);
await context.SaveChangesAsync();
var liamCheck = context.Customers
    .FirstOrDefault(c => c.FirstName == "Liam" && c.LastName == "Chen");
if (liamCheck is null)
{
    Console.WriteLine("Liam successfull removed");
}
var allOrders = context.Orders;

totalRevenue = context.Orders
    .Sum(o => o.TotalAmount);
Console.WriteLine($"Total revenue is ${totalRevenue}");


//Activity 10: List all customers sorted by last name
Console.WriteLine("\n== Activity 10 ==");
var sortedCustomers = context.Customers
    .OrderBy(c => c.LastName)
    .ToList();
foreach (var customer in sortedCustomers)
{
    Console.WriteLine($"- {customer.LastName}, {customer.FirstName}");
}



// ====================== HELPER METHOD ======================
void SeedData(CrmContext ctx)
{
    if (ctx.Customers.Any()) return;

    var customers = new List<Customer>
    {
        new Customer 
        { 
            FirstName = "Emma", LastName = "Johnson", Email = "emma.j@email.com", 
            Phone = "555-1234", DateOfBirth = new DateTime(1995, 3, 15),
            Orders = new List<Order>
            {
                new Order { OrderDate = DateTime.Now.AddDays(-30), TotalAmount = 249.99m, Status = "Completed" },
                new Order { OrderDate = DateTime.Now.AddDays(-5),  TotalAmount = 89.50m,  Status = "Pending" }
            }
        },
        new Customer 
        { 
            FirstName = "Liam", LastName = "Chen", Email = "liam.chen@email.com", 
            Phone = "555-5678", DateOfBirth = new DateTime(1988, 7, 22),
            Orders = new List<Order> { new Order { OrderDate = DateTime.Now.AddDays(-15), TotalAmount = 1249.00m, Status = "Completed" } }
        },
        new Customer 
        { 
            FirstName = "Sophia", LastName = "Rodriguez", Email = "sophia.r@email.com", 
            Phone = "555-9012", DateOfBirth = new DateTime(2000, 11, 8)
        }
    };

    ctx.Customers.AddRange(customers);
    ctx.SaveChanges();
    Console.WriteLine("Seeded 3 customers with sample orders.\n");
}

// ====================== MODELS & CONTEXT ======================
public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public List<Order> Orders { get; set; } = new();
}

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
}

public class CrmContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("CrmDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);
    }
}
