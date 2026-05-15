using Microsoft.EntityFrameworkCore;

namespace TodoApiEf;
public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) {}

    public DbSet<Todo> Todos => Set<Todo>();
}