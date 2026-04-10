public class Barista
{
    private readonly ICoffeeMachine _coffeeMachine;

    public Barista(ICoffeeMachine coffeeMachine)
    {
        _coffeeMachine = coffeeMachine;
    }

    public async Task<bool> TakeOrderAsync(string drink, bool addMilk, bool addSugar)
    {
        var order = new CoffeeOrder 
        { 
            Drink = drink, 
            HasMilk = addMilk, 
            HasSugar = addSugar 
        };

        bool success = await _coffeeMachine.MakeCoffeeAsync(order);
        
        if (success)
            order.IsCompleted = true;

        return success;
    }

    public async Task<bool> CanServeCustomersAsync()
    {
        int stock = await _coffeeMachine.GetCoffeeStockAsync();
        return stock > 5;
    }
}