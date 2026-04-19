public interface ITaskRepository
{
    Task<TaskItem?> GetTaskAsync(int taskId);
    Task<List<TaskItem>> GetTasksByAssigneeAsync(string assignee);
    Task AddTaskAsync(TaskItem task);
    Task UpdateTaskAsync(TaskItem task);
    Task DeleteTaskAsync(int taskId);
}
