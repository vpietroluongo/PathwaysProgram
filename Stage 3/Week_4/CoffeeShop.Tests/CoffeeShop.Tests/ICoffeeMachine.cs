public interface ICoffeeMachine
{
    Task<bool> MakeCoffeeAsync(CoffeeOrder order);
    Task<int> GetCoffeeStockAsync();
}
