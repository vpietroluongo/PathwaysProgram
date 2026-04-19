public interface INotificationService
{
    Task SendOrderConfirmationAsync(string phoneNumber, int orderId);
    Task SendReadyForPickupAsync(string phoneNumber, int orderId);
}
