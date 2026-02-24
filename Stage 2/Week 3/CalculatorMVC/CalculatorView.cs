using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CalculatorMVC;

class CalculatorView
{
    public double Result
        { get; set; }
    public CalculatorView()    //constructor
    { 
        Result = double.MinValue; 
    }
    public void CalculatorViewHeading()
    {
        Console.WriteLine("Console Calculator in C#");
        Console.WriteLine("------------------------\n");
    }
    public bool DetermineGoAgain()
    {
        bool endApp = false;
        Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
        if (Console.ReadLine() == "n") endApp = true;
        return endApp;
    }

    public double GetNumber()
    {
        Console.Write("Type a number, and then press Enter: ");
        string input = Console.ReadLine();
        double validNumber = 0;
        while (!double.TryParse(input, out validNumber))
        {
            Console.Write("This is not a valid input.  Please enter a number: ");
            input = Console.ReadLine();
        }

        return validNumber;
    }

    public string GetOperator()
    {
        Console.WriteLine("Choose an operator from the following list:");
        Console.WriteLine("\ta - Add");
        Console.WriteLine("\ts - Subtract");
        Console.WriteLine("\tm - Multiple");
        Console.WriteLine("\td - Divide");
        Console.Write("Your option: ");

        string input = (Console.ReadLine()).ToLower();
        bool validInput = false;

        while (!("asmd".Contains(input)))
        {
            Console.Write("This is not a valid input.  Please enter a, s, m, or d: ");
            input = (Console.ReadLine()).ToLower();
        }

        while (!validInput)
        {
            switch (input)
            {
                case "a":
                    validInput = true;
                    break;
                case "s":
                    validInput = true;
                    break;
                case "m":
                    validInput = true;
                    break;
                case "d":
                    validInput = true;
                    break;
                default:
                    Console.Write("This is not a valid input.  Please enter a, s, m, or d: ");
                    input = (Console.ReadLine()).ToLower();
                    break;
            }
        }

        return input;
    }

    public void ShowResult()
    {
        if (Result != double.MinValue)
        {
            Console.WriteLine($"Your result is {Result}");
        }
        else
        {
            Console.WriteLine("Cannot divide by 0.");
        }
    }

}