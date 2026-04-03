using System;
using System.Linq.Expressions;

namespace Day1;
public class Calculator
{
    public int Number1
    { get; set; }

    public int Number2
    { get; set; }

    public Calculator()
    {
        Number1 = 0;
        Number2 = 0;
    }

    public Calculator(int number1, int number2)
    {
        Number1 = number1;
        Number2 = number2;
    }
    public int Add()
    {
        return Number1 + Number2;
    }

    public int Subtract()
    {
        return Number1 - Number2;
    }

    public int Multiply()
    {
        return Number1 * Number2;
    }

    public int Divide()
    {
        try
        {
            return Number1 / Number2;
        }
        catch (DivideByZeroException e)
        {
            Console.WriteLine($"Cannot divide by zero.  Returning 0. Error message: {e.Message}");
            return 0;
        }
    }
}