using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new CrmContext();

        // Ensure database is created and seed data
        await SeedDataAsync(context);

        Console.WriteLine("=== CRM Demo - EF Model Builder Examples ===\n");

        // LINQ Query 1: Get all Active Customers with their interactions
        var activeCustomers = await context.Customers
            .Include(c => c.Interactions)
            .Where(c => c.Status == CustomerStatus.ActiveCustomer)
            .OrderBy(c => c.LastName)
            .ToListAsync();

        Console.WriteLine("Active Customers:");
        foreach (var customer in activeCustomers)
        {
            Console.WriteLine($"- {customer.FirstName} {customer.LastName} | Lifetime Value: ${customer.LifetimeValue}");
            Console.WriteLine($"  Interactions: {customer.Interactions.Count}");
        }

        // LINQ Query 2: Customers with high lifetime value
        var highValue = await context.Customers
            .Where(c => c.LifetimeValue > 10000)
            .Select(c => new 
            { 
                c.FullName, 
                c.Email,
                c.Phone, 
                c.Status 
            })
            .ToListAsync();

        Console.WriteLine("\nHigh Value Customers ($10,000+):");
        highValue.ForEach(c => Console.WriteLine($"  {c.FullName} - {c.Email}, {c.Phone}"));

        // LINQ Query 3: Group by Status
        var statusSummary = await context.Customers
            .GroupBy(c => c.Status)
            .Select(g => new 
            { 
                Status = g.Key, 
                Count = g.Count(),
                AvgLifetimeValue = g.Average(c => c.LifetimeValue)
            })
            .ToListAsync();

        Console.WriteLine("\nCustomer Summary by Status:");
        foreach (var group in statusSummary)
        {
            Console.WriteLine($"  {group.Status}: {group.Count} customers | Avg Value: ${group.AvgLifetimeValue:F2}");
        }

        //Query only deleted customers
        var deletedCustomers = context.Customers
            .IgnoreQueryFilters()  // Bypasses the global filter
            .Where(c => c.Status == CustomerStatus.Lost)
            .ToList();
        
        Console.WriteLine("\nDeleted customers:");
        foreach (var customer in deletedCustomers)
        {
            Console.WriteLine($"  {customer.FirstName} {customer.LastName}: {customer.Status}");
            customer.Status = CustomerStatus.Prospect;
            await context.SaveChangesAsync();
        }

        //Query shadow property
        var recentCustomers = context.Customers
            .Where(c => EF.Property<DateTime>(c, "LastModifiedDate") > DateTime.UtcNow.AddDays(-7))
            .ToList();
        
        Console.WriteLine("\nRecent customers:");
        foreach (var customer in recentCustomers) 
        {
            Console.WriteLine($"  {customer.FirstName} {customer.LastName}: {customer.Status}");
        }

        //Query tags
        var vipCustomers = context.Customers
            .Include(c => c.Tags)
            .Where(c => c.Tags.Any(t => t.Name == "VIP"))
            .ToList();
        
        Console.WriteLine("\nVIP customers:");
        foreach (var customer in vipCustomers) 
        {
            Console.WriteLine($"  {customer.FirstName} {customer.LastName}");
        }

        Console.WriteLine("\nModel Builder configuration applied successfully!");
        Console.ReadKey(); 
    }

    static async Task SeedDataAsync(CrmContext context)
    {
        if (await context.Customers.AnyAsync()) return;

        var customers = new List<Customer>
        {
            new Customer 
            { 
                FirstName = "Emma", LastName = "Wilson", Email = "emma.w@email.com", 
                //Phone = "555-0101", DateOfBirth = new DateTime(1990, 5, 15), 
                Phone = new PhoneNumber("555-0101"), DateOfBirth = new DateTime(1990, 5, 15),
                Status = CustomerStatus.ActiveCustomer, LifetimeValue = 12500.75m 
            },
            new Customer 
            { 
                FirstName = "Liam", LastName = "Chen", Email = "liam.chen@email.com", 
                //Phone = "555-0202", DateOfBirth = new DateTime(1985, 3, 22), 
                Phone = new PhoneNumber("555-0202"), DateOfBirth = new DateTime(1985, 3, 22),
                Status = CustomerStatus.Prospect, LifetimeValue = 4500m 
            },
            new Customer 
            { 
                FirstName = "Olivia", LastName = "Rodriguez", Email = "olivia.r@email.com", 
                //Phone = "555-0303", DateOfBirth = new DateTime(1995, 11, 8), 
                Phone = new PhoneNumber("555-0303"), DateOfBirth = new DateTime(1995, 11, 8),
                Status = CustomerStatus.ActiveCustomer, LifetimeValue = 28900m 
            },
            new Customer 
            { 
                FirstName = "Bob", LastName = "Smith", Email = "bob.s@email.com", 
                //Phone = "555-0304", DateOfBirth = new DateTime(1995, 11, 8), 
                Phone = new PhoneNumber("555-0304"), DateOfBirth = new DateTime(1995, 11, 8), 
                Status = CustomerStatus.Lost, LifetimeValue = 28900m 
            }
        };

        context.Customers.AddRange(customers);

        var tags = new List<Tag>
        {
            new Tag { Name = "VIP"},
            new Tag { Name = "Repeat"},
            new Tag { Name = "Discount Hunter"}
        };

        context.Tags.AddRange(tags);

        customers[0].Tags.Add(tags[0]);
        customers[0].Tags.Add(tags[1]);
        customers[1].Tags.Add(tags[0]);
        customers[2].Tags.Add(tags[2]);
        
        await context.SaveChangesAsync();

        // Add some interactions
        var interaction = new Interaction
        {
            CustomerId = 1,
            InteractionDate = DateTime.UtcNow.AddDays(-3),
            Type = "Meeting",
            Notes = "Discussed enterprise solution",
            DurationMinutes = 45
        };

        context.Interactions.Add(interaction);
        await context.SaveChangesAsync();
    }
}

// Helper property for cleaner display
public partial class Customer
{
    public string FullName => $"{FirstName} {LastName}";
}