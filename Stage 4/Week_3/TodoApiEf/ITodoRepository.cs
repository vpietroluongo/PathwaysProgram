using TodoApiEf;

namespace TodoApiEf;
public interface ITodoRepository
{
    Task<List<Todo>> GetAllAsync();
    Task<Todo> GetByIdAsync(int id);
    Task<Todo> AddAsync(Todo task);
    Task UpdateAsync(Todo task);
    Task DeleteAsync(int id);
}