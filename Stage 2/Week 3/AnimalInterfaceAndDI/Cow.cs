using System;


namespace AnimalInterfaceAndDI;

internal class Cow : ISound
{
    public void AnimalSound()
    {
        Console.WriteLine("Mooooo");
    }
}
