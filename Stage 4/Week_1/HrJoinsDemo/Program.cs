using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new HrContext();

        await SeedDataAsync(context);

        Console.WriteLine("===HR System - Inner Joins Demo ===\n");

        //1. Get all employees with their department name: Inner Join Employees with Department Name
        Console.WriteLine("\n=== Activity 1 ===");
        var employeesWithDept = await context.Employees
            .Include(e => e.Department)   //Eager loading (inner join equivalent)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                Department = e.Department.Name,
                e.Salary
            })
            .ToListAsync();
        
        foreach (var emp in employeesWithDept)
        {
            Console.WriteLine($"{emp.FirstName} {emp.LastName} - {emp.Department} ${emp.Salary}");
        }


        //2. Find employees who earn more than $80,000 and show their department: Inner Join with filter
        Console.WriteLine("\n=== Activity 2 ===");
        var highEarners = await (from e in context.Employees
                                join d in context.Departments
                                on e.DepartmentId equals d.Id
                                where e.Salary > 80000
                                select new
                                {
                                    FullName = e.FirstName + " " + e.LastName,
                                    Department = d.Name,
                                    e.Salary
                                }).ToListAsync();
        
        foreach (var emp in highEarners)
        {
            Console.WriteLine($"{emp.FullName} - {emp.Department} ${emp.Salary}");
        }


        //3. List departments that have at least one employee: Inner Join
        Console.WriteLine("\n=== Activity 3 ===");
        var activeDepts = await context.Departments
            .Join(context.Employees,
                d => d.Id,
                e => e.DepartmentId,
                (d,e) => new {DepartmentName = d.Name})
            .Distinct()
            .ToListAsync();
        
        activeDepts.ForEach(d => Console.WriteLine($"{d.DepartmentName}"));


        //4. Show the highest paid employee in each depertment
        Console.WriteLine("\n=== Activity 4 ===");
        var topEarners = await (from d in context.Departments
                                join e in context.Employees
                                on d.Id equals e.DepartmentId
                                group e by d.Name into g
                                select new
                                {
                                    Department = g.Key,
                                    Employee = g.OrderByDescending(x => x.Salary).FirstOrDefault()
                                }).ToListAsync();
        
        foreach (var item in topEarners)
        {
            if (item.Employee != null)
            {
                Console.WriteLine($"{item.Department}: {item.Employee.FirstName} {item.Employee.LastName} ${item.Employee.Salary}");
            }
        }


        //5. Count how many employees are in each department
        Console.WriteLine("\n=== Activity 5 ===");
        var deptCounts = await context.Departments
            .Join(context.Employees,
                d => d.Id,
                e => e.DepartmentId,
                (d, e) => new {d.Name})
            .GroupBy(x => x.Name)
            .Select(g => new
            {
                Department = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        foreach (var dept in deptCounts)
        {
            Console.WriteLine($"{dept.Department}: {dept.Count} employees");
        }


        //6. Rewrite one query using only navigation properties instead of explicit Join()
        Console.WriteLine("\n=== Activity 6 ===");
        var deptCounts2 = await context.Employees
            .GroupBy(e => e.Department.Name)
            .Select(g => new
            {
                Department = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
        
        foreach (var dept in deptCounts2)
        {
            Console.WriteLine($"{dept.Department}: {dept.Count} employees");
        }


        //7. Add a Project entity and create a many-to-many join with Employees
        Console.WriteLine("\n=== Activity 7 ===");
        var employeeProjects = await context.Projects
            .Include(p => p.Employees)
            .OrderBy(p => p.Name)
            .SelectMany(p => p.Employees, (p, e) => new
            {
                Employee = e.FirstName + " " + e.LastName,
                Project = p.Name,
                p.Budget
            })
            .ToListAsync();
        

        foreach (var ep in employeeProjects)
        {
            Console.WriteLine($"{ep.Employee}: {ep.Project} with ${ep.Budget} budget");
        }
        
        var grouped = employeeProjects.GroupBy(ep => ep.Project);
        foreach (var group in grouped)
        {
            Console.WriteLine($"\nProject: {group.Key}, Budget: ${group.First().Budget}");
            foreach (var ep in group)
            {
                Console.WriteLine($"     {ep.Employee}");
            }
        }


        //8. Write a query that shows employees who have no department (Left Join simulation)
        Console.WriteLine("\n=== Activity 8 ==="); 
        var noDept = await (from e in context.Employees
                            join d in context.Departments
                            on e.DepartmentId equals d.Id into deptGroup
                            from d in deptGroup.DefaultIfEmpty()
                            where d == null
                            select new
                            {
                                FullName = e.FirstName + " " + e.LastName,
                            }).ToListAsync();
        // var noDept = await context.Employees
        //     .Join(context.Departments,
        //         e => e.DepartmentId,
        //         d => d.Id,
        //         (e, d) => new {d.Name}) 
        //     .Select(x => new
        //     {
        //         FullName = x.FirstName + " " + x.LastName  //not getting this to work
        //     })
        //     .ToListAsync();
        foreach (var employee in noDept)
        {
            Console.WriteLine(employee.FullName);
        }

        //note: had to make DepartmentId nullable in Employee to get Frank to show up in allEmployeesWDept
        var allEmployeesWDept = await context.Employees
            .Include(e => e.Department)
            .Select(e => new
            {
                FullName = e.FirstName + " " + e.LastName,
                Department = e.Department != null ? e.Department.Name : "No Deparment",
                e.Salary
            })
            .ToListAsync();
        foreach (var employee in allEmployeesWDept)
        {
            Console.WriteLine($"{employee.FullName}, Department: {employee.Department}, Salary: {employee.Salary} ");
        }
    }   

    static async Task SeedDataAsync(HrContext context)
    {
        if (await context.Employees.AnyAsync()) return;

        var departments = new List<Department>
        {
            new Department { Id = 1, Name = "Engineering", Location = "New York" },
            new Department { Id = 2, Name = "Marketing", Location = "Chicago" },
            new Department { Id = 3, Name = "Finance", Location = "Boston" }
        };

        var projects = new List<Project>
        {
            new Project { Name = "Onboarding Redesign", Budget = 250000 },
            new Project { Name = "Website Redesign", Budget = 120000.00m },
            new Project { Name = "Payroll Automation", Budget = 180000.00m }
        };

        var employees = new List<Employee>
        {
            new Employee { FirstName = "Alice", LastName = "Smith", Email = "alice@company.com", Salary = 95000, DepartmentId = 1 },
            new Employee { FirstName = "Bob", LastName = "Johnson", Email = "bob@company.com", Salary = 82000, DepartmentId = 1 },
            new Employee { FirstName = "Carol", LastName = "Williams", Email = "carol@company.com", Salary = 72000, DepartmentId = 2 },
            new Employee { FirstName = "David", LastName = "Brown", Email = "david@company.com", Salary = 110000, DepartmentId = 3 },
            new Employee { FirstName = "Emma", LastName = "Davis", Email = "emma@company.com", Salary = 65000, DepartmentId = 2 },
            new Employee { FirstName = "Frank", LastName = "Smith", Email = "frank@company.com", Salary = 95000 }
        };

        employees[0].Projects.Add(projects[0]);
        employees[0].Projects.Add(projects[1]);
        employees[1].Projects.Add(projects[0]);
        employees[2].Projects.Add(projects[1]);
        employees[3].Projects.Add(projects[2]);
        employees[4].Projects.Add(projects[1]);

        context.Departments.AddRange(departments);
        context.Projects.AddRange(projects);
        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();
    }
}
