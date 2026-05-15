using System.Globalization;

namespace TipCalculatorPromptC;

internal static class Program
{
	private static void Main()
	{
		Console.WriteLine("================================");
		Console.WriteLine("      Professional Tip Tool     ");
		Console.WriteLine("================================");
		Console.WriteLine("Type 'exit' at the bill prompt to quit.");

		while (true)
		{
			if (!InputReader.TryReadPositiveDecimalOrExit("Enter bill amount: ", out decimal billAmount))
			{
				Console.WriteLine("Goodbye.");
				break;
			}

			ServiceQuality quality = InputReader.ReadServiceQuality();
			decimal tipPercentage = quality == ServiceQuality.Custom
				? InputReader.ReadCustomTipPercentage()
				: TipCalculator.GetStandardTipPercentage(quality);

			TipCalculationResult result = TipCalculator.Calculate(billAmount, tipPercentage, quality);

			Console.WriteLine();
			Console.WriteLine("Calculation Summary");
			Console.WriteLine("-------------------");
			Console.WriteLine($"Bill Amount   : {result.BillAmount.ToString("C", CultureInfo.CurrentCulture)}");
			Console.WriteLine($"Service Level : {result.ServiceQuality}");
			Console.WriteLine($"Tip Percentage: {result.TipPercentage:P2}");
			Console.WriteLine($"Tip Amount    : {result.TipAmount.ToString("C", CultureInfo.CurrentCulture)}");
			Console.WriteLine($"Total Amount  : {result.TotalAmount.ToString("C", CultureInfo.CurrentCulture)}");
			Console.WriteLine();
		}
	}
}

internal enum ServiceQuality
{
	Poor = 1,
	Good = 2,
	Excellent = 3,
	Custom = 4
}

internal readonly record struct TipCalculationResult(
	decimal BillAmount,
	decimal TipPercentage,
	decimal TipAmount,
	decimal TotalAmount,
	ServiceQuality ServiceQuality);

internal static class TipCalculator
{
	private static readonly IReadOnlyDictionary<ServiceQuality, decimal> TipPercentages =
		new Dictionary<ServiceQuality, decimal>
		{
			[ServiceQuality.Poor] = 0.05m,
			[ServiceQuality.Good] = 0.15m,
			[ServiceQuality.Excellent] = 0.20m
		};

	public static decimal GetStandardTipPercentage(ServiceQuality serviceQuality)
	{
		if (!TipPercentages.TryGetValue(serviceQuality, out decimal tipPercentage))
		{
			throw new ArgumentException("Service quality does not have a predefined tip percentage.", nameof(serviceQuality));
		}

		return tipPercentage;
	}

	public static TipCalculationResult Calculate(decimal billAmount, decimal tipPercentage, ServiceQuality serviceQuality)
	{
		if (billAmount <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(billAmount), "Bill amount must be greater than zero.");
		}

		if (tipPercentage < 0 || tipPercentage > 1)
		{
			throw new ArgumentOutOfRangeException(nameof(tipPercentage), "Tip percentage must be between 0 and 1.");
		}

		decimal tipAmount = Math.Round(billAmount * tipPercentage, 2, MidpointRounding.AwayFromZero);
		decimal totalAmount = Math.Round(billAmount + tipAmount, 2, MidpointRounding.AwayFromZero);

		return new TipCalculationResult(billAmount, tipPercentage, tipAmount, totalAmount, serviceQuality);
	}
}

internal static class InputReader
{
	public static bool TryReadPositiveDecimalOrExit(string prompt, out decimal amount)
	{
		while (true)
		{
			Console.Write(prompt);
			string? input = Console.ReadLine();

			if (string.Equals(input?.Trim(), "exit", StringComparison.OrdinalIgnoreCase))
			{
				amount = 0;
				return false;
			}

			if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal value) && value > 0)
			{
				amount = value;
				return true;
			}

			Console.WriteLine("Invalid amount. Please enter a positive number or type 'exit'.");
		}
	}

	public static ServiceQuality ReadServiceQuality()
	{
		while (true)
		{
			Console.WriteLine();
			Console.WriteLine("Select service quality:");

			foreach (ServiceQuality quality in Enum.GetValues<ServiceQuality>())
			{
				Console.WriteLine($"{(int)quality}. {quality}");
			}

			Console.Write("Enter choice (1-4): ");
			string? input = Console.ReadLine();

			if (int.TryParse(input, out int selectedValue) && Enum.IsDefined(typeof(ServiceQuality), selectedValue))
			{
				return (ServiceQuality)selectedValue;
			}

			Console.WriteLine("Invalid selection. Please choose a number between 1 and 4.");
		}
	}

	public static decimal ReadCustomTipPercentage()
	{
		while (true)
		{
			Console.Write("Enter custom tip percentage (0-100): ");
			string? input = Console.ReadLine();

			if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal percentage)
				&& percentage >= 0
				&& percentage <= 100)
			{
				return percentage / 100m;
			}

			Console.WriteLine("Invalid percentage. Please enter a value between 0 and 100.");
		}
	}
}
