using System;
using MyBackpack.Services;

namespace MyBackpack.Services;

public class UnitConversionService : IConvert
{
    private readonly IConvert _unitOfMass;

    public UnitConversionService(IConvert unitOfMass)
    {
        _unitOfMass = unitOfMass;
    }

    public double Convert(double value, string fromUnit)
    {
        return _unitOfMass.Convert(value, fromUnit);
    }
}

