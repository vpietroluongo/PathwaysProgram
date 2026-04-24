using Microsoft.EntityFrameworkCore;

public class CrmDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) { }
}