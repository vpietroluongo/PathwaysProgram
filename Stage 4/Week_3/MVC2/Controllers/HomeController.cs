using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;

namespace mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Tasks()
    {
        var tasks = new List<TodoItem>
        {
            new() { Id = 1, Title = "Complete MVC assignment", IsCompleted = false },
            new() { Id = 2, Title = "Review controller and view flow", IsCompleted = true },
            new() { Id = 3, Title = "Write tests for new features", IsCompleted = false },
            new() { Id = 4, Title = "Prepare weekly progress update", IsCompleted = false }
        };

        return View(tasks);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
