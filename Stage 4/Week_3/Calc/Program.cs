using System.Globalization;

Console.WriteLine("Simple Calculator");
Console.WriteLine("Supports: add, subtract");
Console.WriteLine("Type 'q' at any prompt to quit.\n");

while (true)
{
    Console.Write("Operation (add/subtract): ");
    var operationInput = Console.ReadLine()?.Trim().ToLowerInvariant();

    if (operationInput is "q" or "quit")
    {
        break;
    }

    if (operationInput is not ("add" or "subtract"))
    {
        Console.WriteLine("Please enter 'add' or 'subtract'.\n");
        continue;
    }

    if (!TryReadNumber("First number: ", out var firstNumber))
    {
        break;
    }

    if (!TryReadNumber("Second number: ", out var secondNumber))
    {
        break;
    }

    var result = operationInput == "add"
        ? firstNumber + secondNumber
        : firstNumber - secondNumber;

    Console.WriteLine($"Result: {result}\n");
}

Console.WriteLine("Goodbye!");
return;

static bool TryReadNumber(string prompt, out decimal number)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (string.Equals(input, "q", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(input, "quit", StringComparison.OrdinalIgnoreCase))
        {
            number = 0;
            return false;
        }

        if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out number))
        {
            return true;
        }

        Console.WriteLine("Invalid number. Use digits like 10 or 3.5.");
    }
}
