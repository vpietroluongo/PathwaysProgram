using Microsoft.AspNetCore.Mvc;
using TodoMvcApp.Models;

namespace TodoMvcApp.Controllers;

public class TodoController : Controller
{
    private static readonly List<TodoItem> _todos =
    [
        new TodoItem { Id = 1, Title = "Set up MVC project", Description = "Create model, controller, and views", IsDone = true },
        new TodoItem { Id = 2, Title = "Build todo pages", Description = "Add index and CRUD pages", DueDate = DateTime.Today.AddDays(2), IsDone = false }
    ];

    private static int _nextId = 3;

    public IActionResult Index()
    {
        return View(_todos.OrderBy(t => t.IsDone).ThenBy(t => t.DueDate).ToList());
    }

    public IActionResult Details(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo is null)
        {
            return NotFound();
        }

        return View(todo);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TodoItem todo)
    {
        if (!ModelState.IsValid)
        {
            return View(todo);
        }

        todo.Id = _nextId++;
        _todos.Add(todo);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo is null)
        {
            return NotFound();
        }

        return View(todo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, TodoItem todo)
    {
        if (id != todo.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(todo);
        }

        var existing = _todos.FirstOrDefault(t => t.Id == id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Title = todo.Title;
        existing.Description = todo.Description;
        existing.DueDate = todo.DueDate;
        existing.IsDone = todo.IsDone;

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo is null)
        {
            return NotFound();
        }

        return View(todo);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo is not null)
        {
            _todos.Remove(todo);
        }

        return RedirectToAction(nameof(Index));
    }
}
