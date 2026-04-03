using Microsoft.EntityFrameworkCore;
using System;

var context = new SchoolContext();

// Seed data
SeedData(context);

Console.WriteLine("Database ready!\n");

// ====================== ACTIVITY 1: Delete with Cascade ======================
Console.WriteLine("=== Activity 1: Delete Department with Cascade ===");

// TODO: Delete the Math department and see what happens to its courses
// var mathDeparment = context.Departments
//     .Include(d => d.Courses)
//     .FirstOrDefault(d => d.Name == "Mathematics");
// if (mathDeparment is not null)
// {
//     context.Departments.Remove(mathDeparment);
//     context.SaveChanges();
//     Console.WriteLine("Math deparment deleted.");
//     Console.WriteLine($"Remaining courses = {context.Courses.Count()}");
// }


// ====================== ACTIVITY 2: Delete with Cascade Disabled ======================
Console.WriteLine("\n=== Activity 2: Delete Department WITHOUT Cascade ===");

// TODO: Change relationship to NoAction and try deleting again
SeedData(context);


var mathDeparment = context.Departments
    .Include(d => d.Courses)
    .FirstOrDefault(d => d.Name == "Mathematics");

if (mathDeparment is not null)
{
    //Remove courses before removing deparment
    var mathCourses = context.Courses
        .Where(c => c.DepartmentId == mathDeparment.Id)
        .ToList();
    context.Courses.RemoveRange(mathCourses);
    context.SaveChanges();
    Console.WriteLine("Math courses removed");

    //Remove department
    context.Departments.Remove(mathDeparment);
    context.SaveChanges();
    Console.WriteLine("Math deparment deleted.");
    Console.WriteLine($"Remaining courses = {context.Courses.Count()}");
}

// ====================== HELPER METHODS ======================
void SeedData(SchoolContext ctx)
{
    if (ctx.Departments.Any()) return;

    var math = new Department { Name = "Mathematics" };
    var physics = new Department { Name = "Physics" };

    ctx.Departments.AddRange(math, physics);

    ctx.Courses.AddRange(
        new Course { Title = "Calculus I", Department = math },
        new Course { Title = "Linear Algebra", Department = math },
        new Course { Title = "Mechanics", Department = physics }
    );

    ctx.SaveChanges();
    Console.WriteLine("✅ Seeded 2 departments and 3 courses.\n");
}

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Course> Courses { get; set; } = new();
}

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
}

public class SchoolContext : DbContext
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Course> Courses => Set<Course>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("SchoolDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Default behavior: Cascade Delete is ON for required relationships
        modelBuilder.Entity<Department>()
            .HasMany(d => d.Courses)
            .WithOne(c => c.Department)
            .HasForeignKey(c => c.DepartmentId)
            //.OnDelete(DeleteBehavior.Cascade);   // Change this to see different behavior
            //.OnDelete(DeleteBehavior.Restrict);    //Can also use NoAction
            .OnDelete(DeleteBehavior.NoAction);
    }
}
