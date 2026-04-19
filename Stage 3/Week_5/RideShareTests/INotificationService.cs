public interface INotificationService
{
    Task SendRideConfirmationAsync(string phone, string pickup, string dropoff);
    Task SendDriverAssignedNotificationAsync(string phone, string driverName, string vehicle);
    Task SendRideCancelledNotificationAsync(string phone, string reason);
}
