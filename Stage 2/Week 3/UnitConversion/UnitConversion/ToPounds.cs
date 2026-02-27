using System;


namespace UnitConversion;
public class ToPounds : IConvert
{

    public string ValueLessThanZeroMessage = "Value cannot be less than zero.";
    public string InvalidUnitMessage = "Invalid unit passed.";

    public double Convert(double value, string unit)
    {
        double pounds = 0;
        if (value >= 0)
        {
            switch (unit)
            {
                case "g":
                    pounds = value / 453.6;
                    break;
                case "kg":
                    pounds = value * 2.2046;
                    break;
                case "oz":
                    pounds = value / 16;
                    break;
                default:
                    throw new Exception(InvalidUnitMessage);
            }
            return pounds;
        }
        else
        {
            throw new ArgumentOutOfRangeException("pounds", pounds, ValueLessThanZeroMessage);
        }
    }
}
