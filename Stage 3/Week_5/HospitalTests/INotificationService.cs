public interface INotificationService
{
    Task SendAdmissionNotificationAsync(string email, string patientName);
    Task SendDischargeNotificationAsync(string email, string patientName);
    Task SendCriticalAlertAsync(string phone, string message);
}
