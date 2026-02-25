using System;

namespace AnimalInterfaceAndDI;
public class AnimalService : ISound
{
    private readonly ISound _animal;
    
    public AnimalService(ISound animal)
    {
        _animal = animal;
    }

    public void AnimalSound()
    {
        _animal.AnimalSound();
    }
}
