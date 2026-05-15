using Microsoft.EntityFrameworkCore;

namespace TodoApiEf;
public class TodoRepository : ITodoRepository
{
    private readonly TodoDbContext _db;

    public TodoRepository(TodoDbContext db)
    {
        _db = db;
    }
    public async Task<List<Todo>>GetAllAsync()
    {
        return await _db.Todos.ToListAsync();
    }

    public async Task<Todo> GetByIdAsync(int id)
    {
        return await _db.Todos.FirstOrDefaultAsync(td => td.Id == id);
    }

    public async Task<Todo> AddAsync(Todo task)
    {
        _db.Todos.AddAsync(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task UpdateAsync(Todo task)
    {
        _db.Todos.Update(task);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var task = await _db.Todos.FirstOrDefaultAsync(td => td.Id == id);

        _db.Todos.Remove(task);
        await _db.SaveChangesAsync();
    }
}