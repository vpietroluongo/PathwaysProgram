using System;

namespace AnimalInterfaceAndDI;

internal class Cat : ISound
{
    public void AnimalSound()
    {
        Console.WriteLine("Meow");
    }
}
