class Program
{
    static void Main(string[] args)
    {
        var processor = new PaymentProcessor();

        Console.WriteLine("=== Simple Payment System ===\n");

        // Pay with Credit Card
        processor.SetPaymentStrategy(new CreditCardPayment());
        processor.ProcessPayment(49.99m);
        processor.DiscountPayment(49.99m);

        // Pay with PayPal
        processor.SetPaymentStrategy(new PayPalPayment());
        processor.ProcessPayment(29.50m);
        processor.DiscountPayment(29.50m);

        // Pay with Cash
        processor.SetPaymentStrategy(new CashPayment());
        processor.ProcessPayment(15.75m);
        processor.DiscountPayment(15.75m);

        // Pay with Bitcoin
        processor.SetPaymentStrategy(new BitcoinPayment());
        processor.ProcessPayment(17.10m);
        processor.DiscountPayment(17.10m);

        Console.WriteLine("\nStrategy Pattern allows easy switching of payment methods!");
        Console.ReadKey();
    }
}
