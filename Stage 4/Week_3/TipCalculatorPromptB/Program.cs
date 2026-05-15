using System.Globalization;

Console.WriteLine("Tip Calculator");
Console.WriteLine("--------------");

var keepRunning = true;

while (keepRunning)
{
    var billAmount = ReadPositiveDecimal("Enter bill amount: $");
    var (serviceQuality, tipPercent) = ReadServiceQuality();
    var numberOfPeople = ReadPositiveInt("Enter number of people splitting the bill: ");

    var tipAmount = Math.Round(billAmount * (tipPercent / 100m), 2);
    var totalWithTip = Math.Round(billAmount + tipAmount, 2);
    var perPerson = Math.Round(totalWithTip / numberOfPeople, 2);

    Console.WriteLine();
    Console.WriteLine($"Bill: ${billAmount:F2}");
    Console.WriteLine($"Tip ({serviceQuality}, {tipPercent:F0}%): ${tipAmount:F2}");
    Console.WriteLine($"Total with tip: ${totalWithTip:F2}");
    Console.WriteLine($"Per person ({numberOfPeople}): ${perPerson:F2}");
    Console.WriteLine();

    keepRunning = ReadYesNo("Would you like to calculate another tip? (y/n): ");
    Console.WriteLine();
}

Console.WriteLine("Thanks for using Tip Calculator.");

static decimal ReadPositiveDecimal(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();

        if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var value) && value > 0m)
        {
            return value;
        }

        Console.WriteLine("Please enter a valid number greater than 0. Example: 45.50");
    }
}

static (string Quality, decimal TipPercent) ReadServiceQuality()
{
    while (true)
    {
        Console.Write("Enter service quality (poor/good/excellent): ");
        var input = Console.ReadLine()?.Trim().ToLowerInvariant();

        if (input is "poor" or "p")
        {
            return ("Poor", 5m);
        }

        if (input is "good" or "g")
        {
            return ("Good", 15m);
        }

        if (input is "excellent" or "e")
        {
            return ("Excellent", 20m);
        }

        Console.WriteLine("Please enter poor, good, or excellent.");
    }
}

static int ReadPositiveInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();

        if (int.TryParse(input, out var value) && value > 0)
        {
            return value;
        }

        Console.WriteLine("Please enter a whole number greater than 0.");
    }
}

static bool ReadYesNo(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim().ToLowerInvariant();

        if (input is "y" or "yes")
        {
            return true;
        }

        if (input is "n" or "no")
        {
            return false;
        }

        Console.WriteLine("Please enter 'y' or 'n'.");
    }
}
