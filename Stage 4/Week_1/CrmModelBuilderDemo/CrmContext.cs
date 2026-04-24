using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class CrmContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Interaction> Interactions { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public CrmContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("CrmDemoDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // === Model Builder Examples ===

        // 1. Configure Primary Key (already default, but shown for clarity)
        modelBuilder.Entity<Customer>()
            .HasKey(c => c.Id);

        // 2. Configure properties with constraints
        modelBuilder.Entity<Customer>()
            .Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Customer>()
            .Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Customer>()
            .Property(c => c.LifetimeValue)
            .HasPrecision(18, 2);

        // 3. Enum conversion to string
        modelBuilder.Entity<Customer>()
            .Property(c => c.Status)
            .HasConversion(
                v => v.ToString(),
                v => (CustomerStatus)Enum.Parse(typeof(CustomerStatus), v));

        // 4. Index on Email (unique)
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        // 5. One-to-Many Relationship Configuration
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Interactions)
            .WithOne(i => i.Customer)
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);   // Delete interactions when customer is deleted

        // 6. Configure Interaction entity
        modelBuilder.Entity<Interaction>()
            .Property(i => i.Type)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Interaction>()
            .Property(i => i.Notes)
            .HasMaxLength(500);

        // 7. Global query filter (soft delete simulation - optional)
        modelBuilder.Entity<Customer>()
            .HasQueryFilter(c => c.Status != CustomerStatus.Lost);

        //8. Configure "LastModifiedDate" shadow property
        modelBuilder.Entity<Customer>()
            .Property<DateTime>("LastModifiedDate")
            //.HasDefaultValueSql("GETUTCDATE()")
            .HasDefaultValue(DateTime.UtcNow)
            //.HasDefaultValueFactory(() => DateTime.UtcNow)
            .ValueGeneratedOnAddOrUpdate();

        
        //9. Create a many-to-many relationship between Customer and Tag
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Tags)
            .WithMany(t => t.Customers);


        //10. Use value converters for a custom PhoneNumber type
        var phoneConverter = new ValueConverter<PhoneNumber, string>(
            phone => phone.Value,               //to database
            value => new PhoneNumber(value)     //from database
        );

        modelBuilder.Entity<Customer>()
            .Property(c => c.Phone)
            .HasConversion(phoneConverter)
            .HasMaxLength(25)
            .IsRequired();
    }
}