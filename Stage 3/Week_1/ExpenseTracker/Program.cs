using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseTracker;

class Program
{
    static void Main(string[] args)
    {
        string runAgain = "yes";

        while (runAgain == "yes")
        {
            Console.Write("Enter the number of expenses you will enter (1-8): ");
            string input = Console.ReadLine();
            int numberOfExpenses;
            while (!int.TryParse(input, out numberOfExpenses) || numberOfExpenses < 1 || numberOfExpenses > 8)
            {
                Console.Write("Invalid input.  Please enter a number between 1 and 8");
                input = Console.ReadLine();
            }

            List<string> listOfDescriptions = new List<string>();
            List<double> listOfAmounts = new List<double>();
            for (int i = 0; i < numberOfExpenses; i++)
            {
                Console.Write($"Enter description for expense #{i + 1}: ");
                input = Console.ReadLine();
                listOfDescriptions.Add(input);

                Console.Write($"Enter amount for expense #{i + 1}: ");
                input = Console.ReadLine();
                double amount;
                while (!double.TryParse(input, out amount) || amount <= 0)
                {
                    Console.Write("Invalid input.  Please enter a number greater than 0: ");
                    input = Console.ReadLine();
                }
                listOfAmounts.Add(amount);
            }
            
            for (int i = 0; i < numberOfExpenses; i++)
            {
                Console.WriteLine($"Description: {listOfDescriptions[i]} | Amount: ${listOfAmounts[i]:F2}"); 
            }
            double totalAmount = CalculateTotal(listOfAmounts);
            Console.WriteLine($"Total amount: ${totalAmount:F2}");
            
            int maxIndex = FindIndexOfMax(listOfAmounts);
            Console.WriteLine($"The most expensive expense was for {listOfDescriptions[maxIndex]} for ${listOfAmounts[maxIndex]:F2}");

            double averageAmount = CalculateAverage(listOfAmounts);
            Console.WriteLine($"The average expense amount is ${averageAmount:F2}");

            Console.Write("Do you want to run again? (yes/no): ");
            runAgain = (Console.ReadLine()).ToLower();
            while (runAgain != "yes"  && runAgain != "no")
            {
                Console.Write("Invalid input.  Please enter yes or no: ");
                runAgain = (Console.ReadLine()).ToLower();
            }
        }
    }

    static double CalculateTotal(List<double> amounts)
    {
        double sum = 0;
        foreach (double amount in amounts)
        {
            sum += amount;
        }

        return sum;
    }

    static int FindIndexOfMax(List<double> amounts)
    {
        double max = double.MinValue;
        int index = 0;
        for (int i = 0; i < amounts.Count; i++)
        {
            if (amounts[i] > max)
            {
                max = amounts[i];
                index = i;
            }
        }
        
        return index;
    }

    static double CalculateAverage(List<double> amounts)
    {
        double sum = 0;
        foreach (double amount in amounts)
        {
            sum += amount;
        }
        double average = sum / amounts.Count;
        return average;
    }
}