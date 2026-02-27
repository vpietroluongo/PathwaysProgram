using System;


namespace UnitConversion;

public interface IConvert
{
    double Convert(double value, string fromUnit);
}
