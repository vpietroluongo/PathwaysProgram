using Microsoft.EntityFrameworkCore;

public class CustomerService
{
    private readonly CrmDbContext _context;

    public CustomerService(CrmDbContext context)
    {
        _context = context;
    }

    // TODO: Implement the following methods

    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        // Use if/else and arrays/lists
        if (customer == null)
        {
            throw new ArgumentNullException(nameof(customer));
        }

        if (string.IsNullOrWhiteSpace(customer.Email))
        {
            throw new ArgumentException("Email is required");
        }

        string[] validStatuses = {"Lead", "Prospect", "Customer", "Inactive"};
        if (!validStatuses.Contains(customer.Status))
            customer.Status = "Lead";

        customer.CreatedDate = DateTime.UtcNow;

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return customer; 
    }

    public async Task<List<Customer>> GetCustomersByStatusAsync(string status)
    {
        // Use LINQ + switch or if/
        if (string.IsNullOrWhiteSpace(status))
        {
            return await _context.Customers.ToListAsync();
        }

        var customers = await _context.Customers
            .Where(c => c.Status == status)
            .ToListAsync();

        return customers;
    }

    public async Task UpdateCustomerStatusAsync(int customerId, string newStatus)
    {
        // Use switch/case for status validation
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        switch (newStatus)
        {
            case "Lead":
            case "Prospect":
            case "Customer":
            case "Inactive":
                customer.Status = newStatus;
                break;
            default:
                throw new ArgumentException($"Invalid status: {newStatus}");
        }

        await _context.SaveChangesAsync();
    }

    public async Task<Dictionary<string, int>> CountCustomersByStatusAsync()
    {
        // Use loops and dictionary or array
        var customers = await _context.Customers.ToListAsync();

        var counts = new Dictionary<string, int>();
        
        int i = 0;
        while (i < customers.Count)
        {
            string status = customers[i].Status;
            if (counts.ContainsKey(status))
                counts[status]++;
            else    
                counts[status] = 1;
            i++;
        }

        return counts;
    }
}