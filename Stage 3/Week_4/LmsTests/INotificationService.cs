public interface INotificationService
{
    Task SendEnrollmentConfirmationAsync(string email, string courseTitle);
    Task SendCourseFullNotificationAsync(string courseTitle);
}
