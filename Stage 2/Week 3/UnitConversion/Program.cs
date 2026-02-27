using System;

namespace UnitConversion;

class Program
{
    static void Main(string[] args)
    {
        IConvert toPoundsConverter = new ToPounds();


        UnitConversionService unitConversionService1 = new UnitConversionService(toPoundsConverter);
        Console.WriteLine(unitConversionService1.Convert(10.0, "kg"));

    }
}
