public class TodoService
{
    private readonly List<Todo> _todos = new();
    private int _nextId = 1;

    public List<Todo> GetAll() => _todos.ToList();
    public Todo? GetById(int id) => _todos.FirstOrDefault(t => t.Id == id);

    public void Add(Todo todo)
    {
        todo.Id = _nextId++;
        _todos.Add(todo);
    }

    public void ToggleComplete(int id)
    {
        var todo = GetById(id);
        if (todo != null)
            todo.IsCompleted == !todo.IsCompleted;
    }

    public void Delete(int id)
    {
        var todo = GetById(id);
        if (todo != null)
            _todos.Remove(todo);
    }
}