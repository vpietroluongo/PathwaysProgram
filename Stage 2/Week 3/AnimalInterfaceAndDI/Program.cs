using System;

namespace AnimalInterfaceAndDI;

class Program
{
    static void Main(string[] args)
    {
        ISound animal = new Cow();

        AnimalService animalService1 = new AnimalService(animal);
        animalService1.AnimalSound();
    }
}