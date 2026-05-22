using Microsoft.AspNetCore.Mvc;

public class TodoController : Controller
{
    private readonly TodoService _service;
    
    public TodoController(TodoService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var todos = _service.GetAll();
        return View(todos);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Todo todo)
    {
        if (!string.IsNullOrWhiteSpace(todo.Title))
        {
            _service.Add(todo);
            return RedirectToAction("Index");
        }
        return View(todo);
    }

    public IActionResult Toggle(int id)
    {
        _service.ToggleComplete(id);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return RedirectToAction("Index");
    }
}