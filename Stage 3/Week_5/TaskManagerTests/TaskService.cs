public class TaskService
{
    private readonly ITaskRepository _repository;
    private readonly IEmailService _emailService;

    public TaskService(ITaskRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public async Task<int> CreateTaskAsync(string title, string description, string assignedTo)
    {
        var task = new TaskItem
        {
            Title = title,
            Description = description,
            AssignedTo = assignedTo,
            IsCompleted = false,
            CreatedDate = DateTime.UtcNow
        };

        await _repository.AddTaskAsync(task);
        await _emailService.SendTaskAssignedNotificationAsync(assignedTo, title);

        return task.Id;   // In real app this would be set by DB
    }

    public async Task CompleteTaskAsync(int taskId, string completedBy)
    {
        var task = await _repository.GetTaskAsync(taskId);
        if (task == null)
            throw new InvalidOperationException("Task not found");

        task.IsCompleted = true;
        await _repository.UpdateTaskAsync(task);

        await _emailService.SendTaskCompletedNotificationAsync(task.AssignedTo, task.Title);
    }

    public async Task<List<TaskItem>> GetPendingTasksForUserAsync(string assignee)
    {
        var tasks = await _repository.GetTasksByAssigneeAsync(assignee);
        return tasks.Where(t => !t.IsCompleted).ToList();
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        await _repository.DeleteTaskAsync(taskId);
    }
}
