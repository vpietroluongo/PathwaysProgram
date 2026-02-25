using System;

namespace AnimalInterfaceAndDI;
public class Dog : ISound
{
    public void AnimalSound()
    {
        Console.WriteLine("Woof woof");
    } 
}
