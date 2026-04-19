public interface IEmailService
{
    Task SendTaskAssignedNotificationAsync(string email, string taskTitle);
    Task SendTaskCompletedNotificationAsync(string email, string taskTitle);
}