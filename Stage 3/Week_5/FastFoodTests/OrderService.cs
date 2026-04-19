public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IKitchenService _kitchenService;
    private readonly INotificationService _notificationService;

    public OrderService(IOrderRepository repository, IKitchenService kitchenService, INotificationService notificationService)
    {
        _repository = repository;
        _kitchenService = kitchenService;
        _notificationService = notificationService;
    }

    public async Task<int> PlaceOrderAsync(string customerName, string phoneNumber, List<string> items, decimal totalAmount)
    {
        var order = new Order
        {
            CustomerName = customerName,
            PhoneNumber = phoneNumber,
            Items = items,
            TotalAmount = totalAmount,
            IsPrepared = false,
            OrderTime = DateTime.UtcNow
        };

        await _repository.AddOrderAsync(order);

        bool canPrepare = await _kitchenService.CanPrepareOrderAsync(items);
        if (!canPrepare)
            throw new InvalidOperationException("Kitchen cannot prepare this order at the moment");

        await _notificationService.SendOrderConfirmationAsync(phoneNumber, order.Id);

        return order.Id;
    }

    public async Task MarkOrderReadyAsync(int orderId)
    {
        var order = await _repository.GetOrderAsync(orderId);
        if (order == null)
            throw new InvalidOperationException("Order not found");

        await _kitchenService.MarkOrderAsPreparedAsync(orderId);

        order.IsPrepared = true;
        await _repository.UpdateOrderAsync(order);

        await _notificationService.SendReadyForPickupAsync(order.PhoneNumber, orderId);
    }

    public async Task<List<Order>> GetPendingOrdersAsync()
    {
        var orders = await _repository.GetPendingOrdersAsync();
        return orders.Where(o => !o.IsPrepared).ToList();
    }
}
