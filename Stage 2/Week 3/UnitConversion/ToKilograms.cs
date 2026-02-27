using System;


namespace UnitConversion;

public class ToKilograms : IConvert
{

    public string ValueLessThanZeroMessage = "Value cannot be less than zero.";
    public string InvalidUnitMessage = "Invalid unit passed.";

    public double Convert(double value, string unit)
    {
        double kilograms = 0;
        if (value >= 0)
        {
            switch (unit)
            {
                case "g":
                    kilograms = value / 1000;
                    break;
                case "lb":
                    kilograms = value * 0.453;
                    break;
                case "oz":
                    kilograms = value * 0.0283495;
                    break;
                default:
                    throw new Exception(InvalidUnitMessage);
            }
            return kilograms;
        }
        else
        {
            throw new ArgumentOutOfRangeException("kilograms", kilograms, ValueLessThanZeroMessage);
        }
    }
}