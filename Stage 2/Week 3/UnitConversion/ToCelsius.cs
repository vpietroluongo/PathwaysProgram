using System;

namespace UnitConversion;

public class ToCelsius : IConvert
{
    public string InvalidUnitMessage = "Invalid unit passed.";

    public double Convert(double value, string unit)
    {
        double celsius = 0;
        switch (unit)
        {
            case "f":
                celsius = (value - 32) / 1.8;
                break;
            case "k":
                 celsius = value - 273.15;
                break;
            default:
                throw new Exception(InvalidUnitMessage);
        }
        return celsius;
    }
}
