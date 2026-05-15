using Microsoft.EntityFrameworkCore;
using TodoApiEf;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseInMemoryDatabase("TodoDb"));

builder.Services.AddScoped<ITodoRepository, TodoRepository>();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// === API Endpoints ===
app.MapGet("/todos", async (ITodoRepository repo) => 
    Results.Ok(await repo.GetAllAsync()));

app.MapGet("/todos/{id}", async (int id, ITodoRepository repo) =>
    await repo.GetByIdAsync(id) is Todo todo 
        ? Results.Ok(todo) 
        : Results.NotFound());

app.MapPost("/todos", async (Todo todo, ITodoRepository repo) =>
{
    var created = await repo.AddAsync(todo);
    return Results.Created($"/todos/{created.Id}", created);
});

app.MapPut("/todos/{id}", async (int id, Todo updated, ITodoRepository repo) =>
{
    await repo.UpdateAsync(updated);
    return Results.NoContent();
});

app.MapDelete("/todos/{id}", async (int id, ITodoRepository repo) =>
{
    await repo.DeleteAsync(id);
    return Results.NoContent();
});

app.Run();