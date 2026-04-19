public interface IKitchenService
{
    Task<bool> CanPrepareOrderAsync(List<string> items);
    Task MarkOrderAsPreparedAsync(int orderId);
}