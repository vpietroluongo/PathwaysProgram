using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorMVC;

class CalculatorModel
{
    public double Number1
        { get; set; }

    public double Number2 
        { get; set; }   

    public string Operator
        { get; set; }

    public CalculatorModel()
    {
        Number1 = 0;
        Number2 = 0;
        Operator = "";
    }

    public CalculatorModel(double num1, double num2, string op)
    {
        Number1 = num1;
        Number2 = num2;
        Operator = op;
    }

    public double CalculateResult()
    {
        switch (Operator)
        {
            case "a":
                return Number1 + Number2;
            case "s":
                return Number1 - Number2;
            case "m":
                return Number1 * Number2;
            case "d":
                if (Number2 != 0)
                {
                    return Number1 / Number2;
                }
                break;
        }

        return double.MinValue;
    }
}