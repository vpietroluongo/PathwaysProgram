public class PaymentProcessor
{
    private IPaymentStrategy _paymentStrategy;

    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        _paymentStrategy = strategy;
    }

    public void ProcessPayment(decimal amount)
    {
        if (_paymentStrategy == null)
        {
            Console.WriteLine("No payment startegy selected!");
            return;
        }

        _paymentStrategy.Pay(amount);
        Console.WriteLine($"Payment completed at {DateTime.UtcNow:yyy-MM-dd HH:mm:ss}");
    }

    public void DiscountPayment(decimal amount)
    {
        if (_paymentStrategy == null)
        {
            Console.WriteLine("No payment strategy selected!");
            return;
        }

        var percentDiscount = .10m;

        amount = amount - (amount * percentDiscount);

        _paymentStrategy.Pay(amount);
    }
}