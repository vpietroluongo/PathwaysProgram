public interface IOrderRepository
{
    Task<Order?> GetOrderAsync(int orderId);
    Task AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task<List<Order>> GetPendingOrdersAsync();
}
