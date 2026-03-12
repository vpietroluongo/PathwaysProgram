using System;


namespace MyBackpack.Services;

public interface IConvert
{
    double Convert(double value, string fromUnit);
}
